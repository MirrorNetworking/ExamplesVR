using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRHealthChild : MonoBehaviour
{
    public VRNetworkHealth vrNetworkHealth;

    [ServerCallback]
    void OnTriggerEnter(Collider _collider)
    {
        //Debug.Log(name + " OnTriggerEnter: " + _collider.name);
        if (vrNetworkHealth)
        {
            //Debug.Log(name + " OnTriggerEnter vrNetworkHealth.HitEvent: " + _collider.name);
            vrNetworkHealth.HitEvent(_collider);
        }
    }
}
