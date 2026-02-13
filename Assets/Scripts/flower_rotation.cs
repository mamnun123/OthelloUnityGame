using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEditor.PackageManager;
using UnityEngine;
using VRSYS.Core.Networking;

public class flower_rotation : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
    }

    // This code gets the flower to always look at the player
    void Update()
    {
        if (IsServer)
        {
            Debug.Log(NetworkManager.Singleton.ConnectedClients);
            transform.LookAt(GetClosest());
            Debug.Log(GetClosest());
            transform.RotateAround(transform.position, transform.up, 180f);
        }
    }

    private Transform GetClosest()
    {
        ulong closest = 0;
        Debug.Log("Current Client ID: " + NetworkManager.Singleton.LocalClientId);
        /*
        for (int i = 1; i < 4; i++)
            if (NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject != null)
            {
                if (Vector3.Distance(this.transform.position, NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject.transform.position) <
                    Vector3.Distance(this.transform.position, NetworkManager.Singleton.ConnectedClients[(ulong)closest].PlayerObject.transform.position))
                {
                    closest = (ulong)i;
                }
            }
        */
        return GetPlayerTransform(closest);

    }

    // Why isn't this working?
    // For some reason, the host isn't spawning players

    public Transform GetPlayerTransform(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            if (client.PlayerObject != null)
            {
                return client.PlayerObject.transform;
            }
        }

        return null; // player not spawned yet
    }
}

/*
 * What needs to be done:
 * 
 * Make sure that the distance between each user is calculated
 * Then whatever is the closest distance, Rotate to look at that user
 * 
 * Or just get the ID of the host
 * 
 */