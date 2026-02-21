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
    private bool modifierAdded = false;

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

        ladybug.player = args.interactorObject.transform;
        ladybug.transform.position = new Vector3(ladybug.player.transform.position.x + 5f, ladybug.player.transform.position.y, ladybug.player.transform.position.z);
        if (ladybug.playerContact == false)
        {
            
            ladybug.playerContact = true;
        }
    }



    public void OnGrab(SelectEnterEventArgs interactor)
    {
        ladybug.OnGrabLadybug(interactor);
    }
}
