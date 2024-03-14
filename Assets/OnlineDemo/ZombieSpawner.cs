using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZombieSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;
    private int counter = 0;

    void Start()
    {
        // Start spawning zombies repeatedly
        StartCoroutine(SpawnZombiesRepeatedly());
    }

    IEnumerator SpawnZombiesRepeatedly()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);
            //
            // Spawn a zombie at the specified spawn point
            SpawnZombieServerRpc();
        }
    }
    [ServerRpc] 
    void SpawnZombieServerRpc()
    {
        // Instantiate a zombie at the spawn point
        if (counter > 4) { return; }
        GameObject MyZombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        MyZombie.GetComponent<NetworkObject>().Spawn(true);
        counter++;

    }
}
