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
        // if arm health is not set, presume we do not want health/combat features.
        if (vrNetworkPlayerScript.vrPlayerRig.canvasUIPosition == null)
        {
            this.enabled = false;
        }
        else
        {
            // default name and health amount is above player heads
            // here we use that same canvas, and set it to local players arm
            canvasUI.SetParent(vrNetworkPlayerScript.vrPlayerRig.canvasUIPosition);
            canvasUI.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            canvasUI.position = vrNetworkPlayerScript.vrPlayerRig.canvasUIPosition.position;
            vrNetworkPlayerScript.vrPlayerRig.damageTriggerL.SetActive(false);
            vrNetworkPlayerScript.vrPlayerRig.damageTriggerR.SetActive(false);
        }
    }

    [ServerCallback]
    void OnTriggerEnter(Collider _collider)
    {
        HitEvent(_collider);
    }

    public void HitEvent(Collider _collider)
    {
        // A check to stop collider calculations if we have this script disabled
        if (this.enabled == false)
        {
            return;
        }
        //Debug.Log(name + " OnTriggerEnter: " + _collider.name);

        if (_collider.name == "PlayerDamage" || _collider.name == "EnemyDamage")
        {
            // we dont want to punch ourselves, usually..  :D
            if (_collider.transform.root != this.transform.root)
            {
                // enemy or player specific events
                if (vrNetworkEnemy)
                {
                    healthCurrent -= 3;
                    vrNetworkEnemy.FindClosestPlayer();
                    vrNetworkEnemy.AttackEvent();
                }
                else if(vrNetworkPlayerScript)
                {
                    healthCurrent -= 5;
                }

                if (healthCurrent <= 0)
                {
                    healthCurrent = 100;
                    // do some death or reset
                }
                RpcOnHit(_collider.ClosestPoint(transform.position));
            }
        }
    }

    [ClientRpc]
    void RpcOnHit(Vector3 _position)
    {
        hitGameObjectsTemp = Instantiate(hitGameObjects[Random.Range(0, hitGameObjects.Length)], _position, this.transform.rotation);
        Destroy(hitGameObjectsTemp, 1.0f);
    }
}
