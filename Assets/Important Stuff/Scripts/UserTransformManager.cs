using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using VRSYS.Core.Networking;

public class UserTransformManager : MonoBehaviour, INetworkUserCallbacks
{
    public List<Transform> userTransforms;

    private void Awake()
    {
        userTransforms = new List<Transform>();
    }

    public void OnLocalNetworkUserSetup()
    {
        Debug.Log("Transform of local user:");
        Debug.Log(NetworkUser.LocalInstance.transform);
        userTransforms.Add(NetworkUser.LocalInstance.transform);
    }

    public void OnRemoteNetworkUserSetup(NetworkUser user)
    {
        Debug.Log("Transform of remote user:");
        Debug.Log(user.transform);
        userTransforms.Add(user.transform);
    }

    public void OnLocalNetworkUserDisconnect()
    {
        /// ....
    }

    public void OnRemoteNetworkUserDisconnect(NetworkUser user)
    {
        userTransforms.Remove(user.transform);
    }

}
