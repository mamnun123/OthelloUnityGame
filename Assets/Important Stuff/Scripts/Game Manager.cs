using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using VRSYS.Core.Logging;
using VRSYS.Core.Networking;


// Purpose of class: Manages important game data, such as player scores and modifiers.
public class GameManager : NetworkBehaviour, INetworkUserCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public int trashCount; // How much total trash is spawned during the round
    public int ladybugCount; // How many ladybugs are spawned during the round
    public NetworkList<int> scores = new NetworkList<int>(); // All player scores
    public NetworkList<int> modifiers = new NetworkList<int>(); // All player modifiers
    public NetworkVariable<int> trashRemaining = new NetworkVariable<int>(); // Amount of trash currently in the scene


    void Start()
    {
        // Initialises the lists to hold all of the right variables
        for (int i = 0; i < 4; i++)
        {
            scores.Add(0);
            modifiers.Add(1);
        }
    }


    void Update()
    {
        // Despawns ladybugs after all the trash is cleaned up.
        if (trashRemaining.Value == 0)
        {
            despawnLadybugs();
        }
    }

    public void AddPoint(int ID)
    {
        // Adds a point to target player. Triggered when a player puts trash in the right bin.
        if (IsServer) {
            scores[ID] += modifiers[ID];
        }
    }

    public void AddModifier(int ID)
    {
        // Adds a modifier to target player. Triggered when a player collects a ladybug.
        if (IsServer)
        {
            modifiers[ID] += 1;
        }
    }

    public void SubtractModifier(int ID)
    {
        // Takes a modifier away from a player. Triggered when a ladybug is stolen from a player
        if (IsServer && modifiers[ID] > 1)
        {
            modifiers[ID] -= 1;
        }
    }

    public void despawnLadybugs()
    { 
        // Function that despawns the ladybugs. Called when all of the trash is cleaned up.
        if (IsServer)
        {
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            int targetLayer = LayerMask.NameToLayer("Ladybug");
            foreach (GameObject obj in allObjects)
            {
                if (obj.layer == targetLayer)
                {
                    obj.GetComponent<NetworkObject>().Despawn();
                }
            }
        }
    }







    //
    // --------------------- FUNCTIONS SURROUNDING USERS CONNECTING -----------------------------
    // 
    // These functions are used as backup debug.logs for if there's issues in the future.
    //


    // Local user joined and initialized
    public void OnLocalNetworkUserSetup()
    {
        ExtendedLogger.LogInfo(GetType().Name, $"Local user is spawned and initialized. Name: {NetworkUser.LocalInstance.userName.Value}", this);
    }

    //Remote user joined session and is fully initialized
    public void OnRemoteNetworkUserSetup(NetworkUser user)
    {
        ExtendedLogger.LogInfo(GetType().Name, $"Remote user is spawned and initialized. Name: {user.userName.Value}", this);
    }

    //Local user disconnected
    public void OnLocalNetworkUserDisconnected()
    {
        ExtendedLogger.LogInfo(GetType().Name, $"Local user is disconnected.", this);
        Debug.Log("Check five six");
    }

    //Remote user disconnected
    public void OnRemoteNetworkUserDisconnected(NetworkUser user)
    {
        ExtendedLogger.LogInfo(GetType().Name, $"Remote user is disconnected.", this);
        Debug.Log("Check seven eight");
    }
}
