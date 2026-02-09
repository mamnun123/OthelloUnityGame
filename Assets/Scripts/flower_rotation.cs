using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEditor.PackageManager;
using UnityEngine;
using VRSYS.Core.Networking;

public class flower_rotation : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private NetworkUser localUser = NetworkUser.LocalInstance;
    public GameObject prefab;
    public GameManager GAMEMANAGER;
    private NetworkList<ulong> IDs;
    private float closestDistance = -1f;
    private Transform closest;
    public GameObject spawnpoint;

    void Start()
    {
        Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // This code gets the flower to always look at the player
    void Update()
    {
        transform.LookAt(GetClosest());
        Debug.Log(GetClosest());
        transform.RotateAround(transform.position, transform.up, 180f);
        if (IDs == null)
        {
            Debug.Log("It's null");
        }
    }

    Transform GetClosest()
    {
        IDs = GAMEMANAGER.IDs;
        foreach (var item in IDs)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(item, out var client))
            {
                Debug.Log("Player ID");
                Debug.Log(item);
                // Why is this not linked to the player?
                Transform playerTransform = client.PlayerObject.transform;
                Debug.Log(playerTransform);
                if (closestDistance == -1)
                {
                    closestDistance = Vector3.Distance(this.transform.position, playerTransform.position);
                    closest = playerTransform;
                } else
                {
                    if (Vector3.Distance(this.transform.position, playerTransform.position) < closestDistance)
                    {
                        closestDistance = Vector3.Distance(this.transform.position, playerTransform.position);
                        closest = playerTransform;
                    }
                }
                return closest;
            }

        }

        return spawnpoint.transform;

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