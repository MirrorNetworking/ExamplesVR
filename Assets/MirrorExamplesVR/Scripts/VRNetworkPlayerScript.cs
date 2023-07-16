using UnityEngine;
using Mirror;
using System.Collections.Generic;
using TMPro;

public class VRNetworkPlayerScript : NetworkBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;
    public GameObject headModel;
    public GameObject rHandModel;
    public GameObject lHandModel;

    public VRPlayerRig vrPlayerRig;
    public VRNetworkHealth vrNetworkHealth;


    public override void OnStartLocalPlayer()
    {
        // create a link to local vr rig, so that rig can sync to our local network players transforms
        vrPlayerRig = GameObject.FindObjectOfType<VRPlayerRig>();
        vrPlayerRig.localVRNetworkPlayerScript = this;

        // we dont need to see our network representation of hands, or our own headset, it also covers camera without using layers or some repositioning
        headModel.SetActive(false);
        rHandModel.SetActive(false);
        lHandModel.SetActive(false);

        // if no customisation is set, create one.
        if (VRStaticVariables.playerName == "")
        {
            CmdSetupPlayer("Player: " + netId);
        }
        else
        {
            CmdSetupPlayer(VRStaticVariables.playerName);
        }
    }

    // a static global list of players that can be used for a variery of features, one being enemies
    public readonly static List<VRNetworkPlayerScript> playersList = new List<VRNetworkPlayerScript>();

    public override void OnStartServer()
    {
        playersList.Add(this);
    }

    public override void OnStopServer()
    {
        playersList.Remove(this);
    }

    [SyncVar(hook = nameof(OnNameChangedHook))]
    public string playerName = "";
    public TMP_Text textPlayerName;

    void OnNameChangedHook(string _old, string _new)
    {
        //Debug.Log("OnNameChangedHook: " + playerName);
        textPlayerName.text = playerName;
    }

    [Command]
    public void CmdSetupPlayer(string _name)
    {
        //player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name; //+ connectionToClient.connectionId;
    }

    [SyncVar(hook = nameof(OnRightObjectChangedHook))]
    public NetworkIdentity rightHandObject;
    public VRWeapon vrWeaponRight;

    void OnRightObjectChangedHook(NetworkIdentity _old, NetworkIdentity _new)
    {
        if (rightHandObject)
        {
            Debug.Log("Mirror OnRightObjectChangedHook: " + rightHandObject);
            vrWeaponRight = rightHandObject.GetComponent<VRWeapon>();
            vrWeaponRight.vrNetworkPlayerScript = this;
        }
        else
        {
           // vrWeaponRight.vrNetworkPlayerScript = null;
            vrWeaponRight = null;
        }
    }

    // switch to Late/Fixed Update if weirdness happens
    //private void Update()
    //{
    //    if (rightHandObject)
    //    {
    //         rightHandObject.transform.position = rHandTransform.position;
    //    }
    //}

    [Command]
    public void CmdFire()
    {
        if (rightHandObject && vrWeaponRight)
        {
            Debug.Log("Mirror CmdFire");
            RpcOnFire();
            if (isServerOnly)
            {
                OnFire();
            }
        }
    }

    [ClientRpc]
    public void RpcOnFire()
    {
        Debug.Log("Mirror RpcOnFire");
        OnFire();
    }

    public void OnFire()
    {
        Debug.Log("Mirror OnFire");
        if (vrWeaponRight)
        {
            var projectile = (GameObject)Instantiate(vrWeaponRight.projectilePrefab, vrWeaponRight.fireLine.position, vrWeaponRight.fireLine.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(vrWeaponRight.fireLine.forward * 35);
            Destroy(projectile, 10.0f);
        }
    }
}
