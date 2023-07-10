using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRNetworkEnemy : NetworkBehaviour
{
    // bit of enemy ai, acts as a punching bag, faces closest or player who hit it last, does spin attack after X amount of attacks
    public Transform targetTransform;
    private float targetDistance = Mathf.Infinity;
    private Vector3 targetDifference;
    private float squaredDirection;
    private int enemyStatus = 0; // 1 = attack
    private int attackTriggerAmount = 3;
    private int attackTriggerCounter = 0;
    public Transform pivotForAttack;
    private Transform mainCameraTransform;
    private Vector3 targetVector;

    public override void OnStartServer()
    {
        if(mainCameraTransform == null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // LateUpdate so that all camera updates are finished.
    [ServerCallback]
    void LateUpdate()
    {
        if (enemyStatus == 1)
        {
            // enemy aggro amount trigger, do a spin attack
            pivotForAttack.RotateAround(pivotForAttack.position, Vector3.up, 333 * Time.deltaTime);        
        }
        else if (targetTransform)
        {
            targetVector = this.transform.position - targetTransform.position;
            targetVector.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetVector), Time.deltaTime * 5);
        }
        else
        {
            FindClosestPlayer();
        }
    }

    public void FindClosestPlayer()
    {
        //Debug.Log("FindClosestPlayer");

        targetDistance = Mathf.Infinity;

        foreach (VRNetworkPlayerScript go in VRNetworkPlayerScript.playersList)
        {
            targetDifference = go.headTransform.transform.position - transform.position;
            squaredDirection = targetDifference.sqrMagnitude;
            if (squaredDirection < targetDistance)
            {
                targetDistance = squaredDirection;
                targetTransform = go.headTransform.transform;
            }
        }
    }

    public void AttackEvent()
    {
        attackTriggerCounter += 1;
        if (attackTriggerCounter >= attackTriggerAmount)
        {
            attackTriggerCounter = 0;
            StartCoroutine(AttackCooldown(UnityEngine.Random.Range(1.0f, 3.0f)));
        }
    }

    private IEnumerator AttackCooldown(float time)
    {
        enemyStatus = 1;
        yield return new WaitForSeconds(time);
        enemyStatus = 0;
    }
}
