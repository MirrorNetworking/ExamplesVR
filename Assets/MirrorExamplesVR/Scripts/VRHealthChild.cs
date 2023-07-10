using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRHealthChild : MonoBehaviour
{
    // This script is to be placed on child objects with trigger colliders
    // Passes info back upto parent heealth script so it effects main health amount
    public VRNetworkHealth vrNetworkHealth;

    [ServerCallback]
    void OnTriggerEnter(Collider _collider)
    {
        //Debug.Log(name + " OnTriggerEnter: " + _collider.name);
        if (vrNetworkHealth)
        {
            vrNetworkHealth.HitEvent(_collider);
        }
    }
}
