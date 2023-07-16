using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRWeapon : NetworkBehaviour
{
    //[SyncVar(hook = nameof(OnOwnerChangedHook))]
    //public NetworkIdentity objectOwner;
    public VRNetworkPlayerScript vrNetworkPlayerScript;
    public Transform fireLine;
    public GameObject projectilePrefab;

    //void OnOwnerChangedHook(NetworkIdentity _old, NetworkIdentity _new)
    //{
    //    Debug.Log("OnOwnerChangedHook: " + objectOwner);
    //    vrNetworkPlayerScript = objectOwner.GetComponent<VRNetworkPlayerScript>();

    //}

    //// switch to Late/Fixed Update if weirdness happens
    //private void Update()
    //{
    //    if (objectOwner && vrNetworkPlayerScript)
    //    {
    //        this.transform.position = vrNetworkPlayerScript.rHandTransform.position;
    //    }
    //}

    //public void EventPickup()
    //{

    //}

    private void Update()
    {
        if (vrNetworkPlayerScript == null)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //vrNetworkPlayerScript = vrNetworkPlayerScript.GetComponent<VRNetworkPlayerScript>();
            vrNetworkPlayerScript.rightHandObject = this.netIdentity;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            vrNetworkPlayerScript.rightHandObject = null;
        }
    }
}
