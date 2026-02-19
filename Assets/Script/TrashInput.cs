using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static UnityEngine.GraphicsBuffer;

// 1. Define the Types globally so Bins can see them
public enum TrashType { Plastic, Paper, WhiteGlass, GreenGlass, BrownGlass, Landfill, Organic}

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(XRGrabInteractable))]
public class TrashItem : NetworkBehaviour
{
    [Header("Settings")]
    public TrashType myType; // Select "Plastic" in Inspector

    // We remember where we spawned so we can reset if put in wrong bin
    private Vector3 startPosition;

    private NetworkObject netObj;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {

        netObj = GetComponent<NetworkObject>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        Debug.Log("LOUD CHECK");
        //grabInteractable.selectExited.AddListener(OnRelease);
    }



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

    void PickupObject()
    {
        NetworkObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


    // -------------------------------------------------

    private void OnGrab(SelectEnterEventArgs interactor)
    {
        // Request ownership from server for the grabbing player
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        if (netObj.OwnerClientId != localClientId)
        {
            RequestOwnershipServerRpc(localClientId);
        }
    }

    /*
    private void OnRelease(SelectExitEventArgs interactor)
    {
        // Optional: return ownership to server when released
        if (!IsServer)
        {
            ReturnOwnershipToServerServerRpc();
        }
    }
    */

    [ServerRpc(RequireOwnership = false)]
    private void RequestOwnershipServerRpc(ulong clientId)
    {
        netObj.ChangeOwnership(clientId);
    }

    /*
    [ServerRpc]
    private void ReturnOwnershipToServerServerRpc()
    {
        netObj.ChangeOwnership(NetworkManager.ServerClientId);
    }
    */

}
