using UnityEngine;
using System.Collections;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class LadybugTransfer : NetworkBehaviour
{
    public XRSimpleInteractable interactable;
    public ladybug_movement ladybug;

    private void Start()
    {
        interactable.selectEntered.AddListener(OnSelected);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSelected(SelectEnterEventArgs args)
    {
        ladybug.player = args.interactorObject.transform;
        ladybug.transform.position = new Vector3(ladybug.player.transform.position.x + 5f, ladybug.player.transform.position.y, ladybug.player.transform.position.z);
        ladybug.playerContact = true;
    }
}
