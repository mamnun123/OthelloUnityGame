using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using VRSYS.Core.Logging;
using VRSYS.Core.Networking;

public class GameManager : NetworkBehaviour, INetworkUserCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public NetworkVariable<int> test = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkList<ulong> IDs = new NetworkList<ulong>();
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

        foreach (var item in IDs)
        {
            Debug.Log(item);
            Debug.Log("Length");
            Debug.Log(IDs.Count);
        }

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
