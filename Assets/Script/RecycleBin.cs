
using UnityEngine;
using Unity.Netcode;

public class RecycleBin : NetworkBehaviour
{
    public TrashType acceptedType; // Sets to "Plastic" for yellow bin, "Paper" for Blue, Oranic Gray, etc.

    // This triggers when any object with a Collider falls into the bin
    void OnTriggerEnter(Collider other)
    {
        // Only the Server decides game logic to prevent cheating/bugs
        if (!IsServer) return;

        // 1. Check if the object is actually Trash
        TrashItem trash = other.GetComponent<TrashItem>();
        if (trash != null)
        {
            // 2. Check Type
            if (trash.myType == acceptedType)
            {
                Debug.Log("Correct Bin!");
                // Correct! Destroy the object (or add score)
                trash.GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                Debug.Log("WRONG BIN! Respawning...");
                // Wrong! Reset position.
                trash.Respawn();
            }
        }
    }
}
