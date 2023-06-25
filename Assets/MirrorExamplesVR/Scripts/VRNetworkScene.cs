using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class VRNetworkScene : NetworkBehaviour
{
    public VRCanvasHUD vrCanvasHUD;
    public AudioSource audioPushButton;
    public ParticleSystem particlePushButton;

    public void Start()
    {
        if (vrCanvasHUD == null)
        { vrCanvasHUD = GameObject.FindObjectOfType<VRCanvasHUD>(); }
    }

    // we use sync var for number, to new players will automatically be setup with same value as server and previous players
    [SyncVar(hook = nameof(OnIncrementalNumberHook))]
    public int incrementalNumber;
    public Text incrementalNumberText;

    // hook is automatically called on clients when sync var is changed
    void OnIncrementalNumberHook(int _Old, int _New)
    {
        OnIncrementalNumberChanged();
    }

    void OnIncrementalNumberChanged()
    {
        incrementalNumberText.text = incrementalNumber.ToString();
        if (vrCanvasHUD)
        { vrCanvasHUD.SetupInfoText("A button was pressed: " + incrementalNumber); }
    }

    // this is linked with Unitys Push Button gameobject.
    public void IncrementalNumberEvent()
    {
        CmdIncrementalNumber();
    }

    // we use the bypass authority attribute to make things easier
    // this command sends request to server/host to increase the number
    // sync var number gets changed, and calls the hook to update ui
    [Command(requiresAuthority = false)]
    public void CmdIncrementalNumber()
    {
        incrementalNumber += 1;
        // if server is not a client too, hook will not get called, so we have to call it manually (if we want server to update ui etc)
        // it may seem like an extra step, but its game dependant.
        if (isServerOnly)
        { OnIncrementalNumberChanged(); }
    }

    // this is linked with Unitys Push Button gameobject.
    public void GenericNetworkEvent(int _value)
    {
        CmdGenericNetwork(_value);
    }

    [Command(requiresAuthority = false)]
    public void CmdGenericNetwork(int _value)
    {
        RpcGenericNetwork(_value);
    }

    [ClientRpc]
    public void RpcGenericNetwork(int _value)
    {
        if (_value == 1)
        {
            audioPushButton.Play();
        }
        else if (_value == 2)
        {
            particlePushButton.Play();
        }
        else if (_value == 3)
        {
            particlePushButton.Stop();
        }
    }
}
