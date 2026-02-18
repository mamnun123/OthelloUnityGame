using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class ButtonInteract : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /*
     * TO-DO:
     * - Disappear object at the start
     * - When selected, spawns trash, sets gamemanager's mode to be GAME and not LOBBY
     * - ALSO the gamemanager needs to count how much trash is left
     * - Also show the scoreboard & (possibly) a timer that shows the time completed afterwards
     */

    public GameManager GAMEMANAGER;
    public ButtonPress Button;
    public GameObject scoreboard;
    public bool isActive = false;
    public XRSimpleInteractable interactable;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            if (isActive == false && NetworkManager.Singleton.StartServer())
            {
                gameObject.SetActive(true);
                isActive = true;
            }
            // Put the feature here that respawns the start button when the trash is all gone
            if (isActive == true)
            {
                
            }
        }
    }
}
