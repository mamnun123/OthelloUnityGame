using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using VRSYS.Core.Networking;


// Purpose of class: To keep track of user positions. Attached to GAMEMANAGER object.
public class UserTransformManager : MonoBehaviour, INetworkUserCallbacks
{
    public List<Transform> userTransforms; // List that stores the user transforms.

    private void Awake()
    {
        // Assigns variable
        userTransforms = new List<Transform>();
    }

    public void OnLocalNetworkUserSetup()
    {
        // Adds user transform for the local user when they log in
        userTransforms.Add(NetworkUser.LocalInstance.transform);
    }

    public void OnRemoteNetworkUserSetup(NetworkUser user)
    {
        // Adds user transform for remote users when they log in
        userTransforms.Add(user.transform);
    }

    public void OnLocalNetworkUserDisconnect()
    {
        // Removes transform when users log off
        userTransforms.Remove(NetworkUser.LocalInstance.transform);
    }

    public void OnRemoteNetworkUserDisconnect(NetworkUser user)
    {
        // Removes transform when users log off
        userTransforms.Remove(user.transform);
    }



}
