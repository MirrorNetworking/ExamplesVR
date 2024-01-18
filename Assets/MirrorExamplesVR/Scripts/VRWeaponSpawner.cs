using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRWeaponSpawner : NetworkBehaviour
{
    public GameObject weaponPrefab;
    public Transform weaponDefault;
    public int maxWeaponsToSpawn = 20;
    private int currentWeaponsSpawned;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private bool canSpawn = true;

    [ServerCallback]
    private void Start()
    {
        spawnPosition = weaponDefault.position;
        spawnRotation = weaponDefault.rotation;
    }

    [ServerCallback]
    void OnTriggerExit(Collider _collider)
    {
        //Debug.Log(name + " OnTriggerExit: " + _collider.name);

        if (canSpawn && currentWeaponsSpawned < maxWeaponsToSpawn && weaponPrefab && weaponDefault && _collider.CompareTag("Weapon"))
        {
            currentWeaponsSpawned++;
            GameObject obj = Instantiate(weaponPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(obj);

            // cooldown to prevent objects that have multiple colliders triggering the exit
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(0.1f);
        canSpawn = true;
    }
}
