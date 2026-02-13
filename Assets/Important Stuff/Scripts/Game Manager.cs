using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using VRSYS.Core.Logging;
using VRSYS.Core.Networking;

public class GameManager : NetworkBehaviour, INetworkUserCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public NetworkVariable<int> test = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkList<ulong> playerObjects = new NetworkList<ulong>();
    public NetworkList<int> scores = new NetworkList<int>();
    public NetworkList<int> modifiers = new NetworkList<int>();
    public NetworkVariable<int> time = new NetworkVariable<int>();


    void Start()
    {
        
        test.OnValueChanged += OnTestValueChanged;
        for (int i = 0; i < 4; i++)
        {
            scores.Add(0);
            modifiers.Add(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetLocalPlayerNetworkObjectId((networkObjectId) =>
        {
            Debug.Log("Local player's NetworkObjectId (host or client): " + networkObjectId);
        });
    }

    public void AddPoint(int ID)
    {
        if (IsServer) {
            scores[ID] += modifiers[ID];
            Debug.Log("Player 1 Score: " + scores[0]);
            Debug.Log("Player 2 Score: " + scores[1]);
        }
    }

    public void AddModifier(int ID)
    {
        if (IsServer)
        {
            modifiers[ID] += 1;
        }
    }

    public static void GetLocalPlayerNetworkObjectId(Action<ulong> callback)
    {
        // Check if NetworkManager exists
        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("NetworkManager not initialized yet!");
            return;
        }

        // If the player object is already spawned, return immediately
        if (NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            callback?.Invoke(NetworkManager.Singleton.LocalClient.PlayerObject.NetworkObjectId);
            return;
        }

        // Otherwise, wait for the player object to spawn
        void OnNetworkSpawn()
        {
            var playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
            if (playerObject != null)
            {
                callback?.Invoke(playerObject.NetworkObjectId);
                // Unsubscribe after we get it
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        // For hosts and clients: wait until the local client connects
        void OnClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                // Delay a frame to ensure player prefab has spawned
                NetworkManager.Singleton.StartCoroutine(WaitForPlayerObject(OnNetworkSpawn));
            }
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    // Coroutine to wait a frame until the PlayerObject exists
    private static System.Collections.IEnumerator WaitForPlayerObject(Action onReady)
    {
        yield return null; // wait one frame
        onReady?.Invoke();
    }



























    public void Check()
    {
        Debug.Log("User Joined");
    }

    public NetworkVariable<int> GetTest()
    {
        return test;
    }

    public void SetTest(int value)
    {
        RequestSetTestNetVarRpc(value);
    }

    [Rpc(SendTo.Server)]
    private void RequestSetTestNetVarRpc(int value)
    {
        test.Value = value;
    }

    private void OnTestValueChanged(int oldValue, int newValue)
    {
        Debug.Log($"Received new test value: {newValue}. Old value was {oldValue}");
    }



    //
    // --------------------- USERS CONNECTING -----------------------------
    // 



    public void OnLocalNetworkUserSetup()
    {
        ExtendedLogger.LogInfo(GetType().Name, $"Local user is spawned and initialized. Name: {NetworkUser.LocalInstance.userName.Value}", this);
        Debug.Log("Check one two");
        Debug.Log(NetworkManager.Singleton.LocalClient.PlayerObject);
    }

    //Remote user joined session and is fully initialized
    public void OnRemoteNetworkUserSetup(NetworkUser user)
    {
        ExtendedLogger.LogInfo(GetType().Name, $"Remote user is spawned and initialized. Name: {user.userName.Value}", this);
        Debug.Log("Check three four");
        if (IsServer)
        {
            //IDs.Add(user.userId.Value);
            //scores.Add(0);
            //modifiers.Add(1);
            //Debug.Log(user.userId.Value);
        }
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
