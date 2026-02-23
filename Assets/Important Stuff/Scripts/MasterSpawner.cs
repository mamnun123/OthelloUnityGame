using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class MasterSpawner : NetworkBehaviour
{
    [Header("Trash Library")]
    public List<NetworkObject> trashPrefabs; // Drag your Can, Bottle, Paper prefabs here

    [Header("Spawn Locations")]
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    public int itemsPerPlayer = 5;

    // Only the Server (Host) runs this!
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnTrash();
        }
    }

    void SpawnTrash()
    {
        for (int i = 0; i < itemsPerPlayer; i++)
        {
            // 1. Pick a RANDOM item index
            int randomIndex = Random.Range(0, trashPrefabs.Count);
            NetworkObject selectedPrefab = trashPrefabs[randomIndex];

            // 2. Spawn for Player 1 (Offset slightly so they don't stack perfectly)
            Vector3 pos1 = spawnPointP1.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.5f, Random.Range(-0.2f, 0.2f));
            SpawnObject(selectedPrefab, pos1);

            // 3. Spawn SAME item for Player 2
            Vector3 pos2 = spawnPointP2.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.5f, Random.Range(-0.2f, 0.2f));
            SpawnObject(selectedPrefab, pos2);
        }
    }

    void SpawnObject(NetworkObject prefab, Vector3 position)
    {
        // Instantiate creates it in memory
        NetworkObject instance = Instantiate(prefab, position, Quaternion.identity);

        // Spawn makes it appear for everyone on the network
        instance.Spawn();
    }
}

