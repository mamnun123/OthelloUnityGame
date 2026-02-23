using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


// Purpose: Script that fires when the ladybugs are interacted with.
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(XRSimpleInteractable))]
public class LadybugTransfer : NetworkBehaviour
{
    public XRSimpleInteractable interactable; // Interaction script
    public ladybug_movement ladybug; // Script for the ladybug's movement
    public GameManager GAMEMANAGER; // Game Manager


    private void Start()
    {
        GAMEMANAGER = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>(); // Assigns object
        interactable.selectEntered.AddListener(OnSelected); // Sets up features for when the object is interacted with
        interactable.selectEntered.AddListener(OnGrab); // ^
    }


    public void OnSelected(SelectEnterEventArgs args)
    {
        // Assigns the ladybug to the player that interacted with it, as well as moves it to the position to start rotating artound the player.
        ladybug.player = args.interactorObject.transform;
        ladybug.transform.position = new Vector3(ladybug.player.transform.position.x + 5f, ladybug.player.transform.position.y, ladybug.player.transform.position.z);
        if (ladybug.playerContact == false)
        {
            ladybug.playerContact = true;
        }
    }

    public void OnGrab(SelectEnterEventArgs interactor)
    {
        // Helps the ladybug change it's owner when interacted with
        ladybug.OnGrabLadybug(interactor);
    }
}
