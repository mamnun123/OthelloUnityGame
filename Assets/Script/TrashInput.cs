using UnityEngine;
using Unity.Netcode;

// 1. Define the Types globally so Bins can see them
public enum TrashType { Plastic, Paper, Glass, Organic }

public class TrashItem : NetworkBehaviour
{
    [Header("Settings")]
    public TrashType myType; // Select "Plastic" in Inspector

    // We remember where we spawned so we can reset if put in wrong bin
    private Vector3 startPosition;

    public override void OnNetworkSpawn()
    {
        // Remember spawn location
        startPosition = transform.position;
    }

    public void Respawn()
    {
        // Reset position to start
        transform.position = startPosition;

        // Kill velocity so it doesn't keep flying
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
