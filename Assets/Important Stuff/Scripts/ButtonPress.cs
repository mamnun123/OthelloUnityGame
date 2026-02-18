using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Unity.Netcode;

[RequireComponent(typeof(XRSimpleInteractable))]
public class ButtonPress : NetworkBehaviour
{
    public XRSimpleInteractable interactable;
    public bool isPressed = false;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        interactable.selectEntered.AddListener(OnSelected);
        interactable.selectExited.AddListener(OnDeselected); // optional
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        Debug.Log("Object was interacted with!");
    }

    private void OnDeselected(SelectExitEventArgs args)
    {
        Debug.Log("Interaction ended.");
    }
}