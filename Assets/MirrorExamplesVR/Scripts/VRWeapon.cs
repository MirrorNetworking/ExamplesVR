using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class VRWeapon : NetworkBehaviour
{
    //[SyncVar(hook = nameof(OnOwnerChangedHook))]
    //public NetworkIdentity objectOwner;
    public VRNetworkPlayerScript vrNetworkPlayerScript;
    public TMP_Text textAmmo;
    public Transform weaponFireLine;
    public GameObject weaponProjectile;

    public float weaponProjectileSpeed = 35.0f;
    public float weaponProjectileLife = 10f;
    public float weaponFireCooldown = 1.0f;
    public float weaponFireCooldownTime;
    public int weaponAmmo = 99;

    private void Start()
    {
        SetTextAmmo();
    }

    public void SetTextAmmo()
    {
        textAmmo.text = weaponAmmo.ToString();
    }

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
