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
    public NetworkVariable<int> time = new NetworkVariable<int>();


    void Start()
    {

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

    public void Check()
    {
        Debug.Log("User Joined");
    }

    public NetworkVariable<int> GetTest()
    {
        return test;
    }

    public void SetTest(NetworkVariable<int> input)
    {
        this.test.Value = input.Value;
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
            IDs.Add(user.userId.Value);
            scores.Add(0);
            Debug.Log(user.userId.Value);
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
