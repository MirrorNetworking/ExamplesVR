using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class VRNetworkHealth : NetworkBehaviour
{
    public VRNetworkPlayerScript vrNetworkPlayerScript;
    public VRNetworkEnemy vrNetworkEnemy;
    public GameObject[] hitGameObjects;
    private GameObject hitGameObjectsTemp;
    public Transform canvasUI;

    [SyncVar(hook = nameof(OnHealthChangedHook))]
    public int healthCurrent = 100;
    public TMP_Text textHealth;

    void OnHealthChangedHook(int _old, int _new)
    {
       // Debug.Log("OnHealthChangedHook: " + healthCurrent);
        textHealth.text = "HP: " + healthCurrent;
    }

    public GameObject damageTriggerR;
    public GameObject damageTriggerL;

    public override void OnStartLocalPlayer()
    {
        canvasUI.SetParent(vrNetworkPlayerScript.vrPlayerRig.canvasUIPosition);
        canvasUI.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        canvasUI.position = vrNetworkPlayerScript.vrPlayerRig.canvasUIPosition.position;
        vrNetworkPlayerScript.vrPlayerRig.damageTriggerL.SetActive(false);
        vrNetworkPlayerScript.vrPlayerRig.damageTriggerR.SetActive(false);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider _collider)
    {
        HitEvent(_collider);
    }

    public void HitEvent(Collider _collider)
    {
        //Debug.Log(name + " OnTriggerEnter: " + _collider.name);

        if (_collider.name == "PlayerDamage" || _collider.name == "EnemyDamage")
        {
            // we dont want to punch ourselves, usually..  :D
            if (_collider.transform.root != this.transform.root)
            {
                //Debug.Log(name + " OnTriggerEnter 2");
                healthCurrent -= 5;
                if (healthCurrent <= 0)
                {
                    healthCurrent = 100;
                    // do some death or reset
                }
                RpcOnHit(_collider.ClosestPoint(transform.position));

                // enemy specific events
                if (vrNetworkEnemy)
                {
                    vrNetworkEnemy.FindClosestPlayer();
                    vrNetworkEnemy.AttackEvent();
                }
            }
        }
        //Debug.Log(name + " OnTriggerEnter 3");
    }

    //void Update()
    //{
    //    // take input from focused window only
    //    if (!Application.isFocused)
    //        return;

    //    // input for local player
    //    if (isLocalPlayer)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            InputDamageOn();
    //        }
    //        else if (Input.GetKeyUp(KeyCode.Space))
    //        {
    //            InputDamageOff();
    //        }
    //    }
    //}

    public void InputDamageOn()
    {
        //vrNetworkPlayerScript.vrPlayerRig.damageTriggerL.SetActive(true);
        //vrNetworkPlayerScript.vrPlayerRig.damageTriggerR.SetActive(true);
        CmdDamageTrigger(1);
    }

    public void InputDamageOff()
    {
        //vrNetworkPlayerScript.vrPlayerRig.damageTriggerL.SetActive(false);
        //vrNetworkPlayerScript.vrPlayerRig.damageTriggerR.SetActive(false);
        CmdDamageTrigger(0);
    }

    [Command]
    void CmdDamageTrigger(int _status)
    {
        Debug.Log("CmdDamageTrigger: " + _status);
        if (_status == 1)
        {
            damageTriggerR.SetActive(true);
            damageTriggerL.SetActive(true);
        }
        else
        {
            damageTriggerR.SetActive(false);
            damageTriggerL.SetActive(false);
        }
    }

    [ClientRpc]
    void RpcOnHit(Vector3 _position)
    {
        hitGameObjectsTemp = Instantiate(hitGameObjects[Random.Range(0, hitGameObjects.Length)], _position, this.transform.rotation);
        Destroy(hitGameObjectsTemp, 1.0f);
    }
}
