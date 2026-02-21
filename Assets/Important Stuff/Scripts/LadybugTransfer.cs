using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(XRSimpleInteractable))]
public class LadybugTransfer : NetworkBehaviour
{
    public XRSimpleInteractable interactable;
    private NetworkObject netObj;
    public ladybug_movement ladybug;
    public GameManager GAMEMANAGER;
    private int oldClientID;

    private void Start()
    {
        netObj = GetComponent<NetworkObject>();
        GAMEMANAGER = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>();
        interactable.selectEntered.AddListener(OnSelected);
        interactable.selectEntered.AddListener(OnGrab);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSelected(SelectEnterEventArgs args)
    {
        if (ladybug.playerContact == true && ladybug.player != null)
        {
            GAMEMANAGER.SubtractModifier(oldClientID);
        }
        else
        {
            ladybug.playerContact = true;
        }
        ladybug.player = args.interactorObject.transform;
        ladybug.transform.position = new Vector3(ladybug.player.transform.position.x + 5f, ladybug.player.transform.position.y, ladybug.player.transform.position.z);
        oldClientID = ladybug.GetObjectID();
        GAMEMANAGER.AddModifier(oldClientID);
    }



    private void OnGrab(SelectEnterEventArgs interactor)
    {
        // Request ownership from server for the grabbing player
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        if (netObj.OwnerClientId != localClientId)
        {
            RequestOwnershipServerRpc(localClientId);
        }
    }


    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void RequestOwnershipServerRpc(ulong clientId)
    {
        Debug.Log("Laser1");
        if (netObj.IsSpawned)
        {
            Debug.Log("Laser2");
            netObj.ChangeOwnership(clientId);
        }
    }
}
