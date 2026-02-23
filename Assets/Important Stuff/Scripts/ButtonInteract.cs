using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


// Purpose: Manages the display of the title, button, and scoreboard
[RequireComponent(typeof(XRSimpleInteractable))]
public class ButtonInteract : NetworkBehaviour
{

    public GameManager GAMEMANAGER; // Game manager
    public GameObject scoreboard; // Link to the scoreboard child
    public NetworkVariable<bool> isActive = new NetworkVariable<bool>(true); // Boolean that states if the title screen & start button is supposed to be showing
    private Vector3 spawnPos = new Vector3(0, 0, 0); // Vector3 used to "reset" the position of the title screen when it respawns

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreboard.transform.position = spawnPos;

        // Moves the title and button when trash is present
        if (isActive.Value == true)
        {
            transform.position = spawnPos;
        }
        else if (isActive.Value == false)
        {
            transform.position = new Vector3(0, -100f, 0);
        }


        // Makes the button & title screen reappear if all of the trash has been cleaned
        if (isActive.Value == false && GAMEMANAGER.trashRemaining.Value == 0)
        {
            isActive.Value = true;
        }
    }
}
