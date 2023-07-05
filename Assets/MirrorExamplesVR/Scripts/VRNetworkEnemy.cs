using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRNetworkEnemy : NetworkBehaviour
{
    public Transform targetTransform;
    private float targetDistance = Mathf.Infinity;
    private Vector3 targetDifference;
    private float squaredDirection;
    private int enemyStatus = 0; // 1 = attack
    private int attackTriggerAmount = 0;
    public Transform pivotForAttack;

    // LateUpdate so that all camera updates are finished.
    [ServerCallback]
    void LateUpdate()
    {
        if (enemyStatus == 1)
        {
            pivotForAttack.RotateAround(pivotForAttack.position, Vector3.up, 333 * Time.deltaTime);
        }
        else if (targetTransform)
        {
            transform.forward = targetTransform.forward;
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
        attackTriggerAmount += 1;
        if (attackTriggerAmount >= 3)
        {
            attackTriggerAmount = 0;
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
