using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


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
    private NetworkVariable<ulong> currOwner = new NetworkVariable<ulong>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Everyone);

    private void Start()
    {

        netObj = GetComponent<NetworkObject>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void Update()
    {
        if (IsServer)
        {
            Debug.Log("Bwawawawawawawawa" + currOwner);
            netObj.ChangeOwnership(currOwner.Value);
        }
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


    // -------------------------------------------------

    private void OnGrab(SelectEnterEventArgs interactor)
    {
        Debug.Log("Checkity check check");
        // Request ownership from server for the grabbing player
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        if (!HasAuthority)
        {
            ChangeDaOwnership(localClientId);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void ChangeDaOwnership(ulong newID)
    {
        Debug.Log("Imma firin my lazuh" + newID);
        currOwner.Value = newID;
    }
}
