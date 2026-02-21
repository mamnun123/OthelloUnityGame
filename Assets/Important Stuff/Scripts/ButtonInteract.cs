using Unity.Netcode;
using Unity.VisualScripting;
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
    public GameObject scoreboard;
    public NetworkVariable<bool> isActive = new NetworkVariable<bool>(true);
    private Transform saveTransform;
    private Vector3 spawnPos = new Vector3(0, 0, 0);

    void Start()
    {
        saveTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        scoreboard.transform.position = spawnPos;
        Debug.Log("Trash remaining: " + GAMEMANAGER.trashRemaining.Value);
        if (isActive.Value == true)
        {
            transform.position = new Vector3 (0, 0, 0);
        }
        else if (isActive.Value == false)
        {
            Debug.Log("Fire!");
            transform.position = new Vector3(0, -100f, 0);
        }


        // Put the feature here that respawns the start button when the trash is all gone
        if (isActive.Value == false && GAMEMANAGER.trashRemaining.Value == 0)
        {
            isActive.Value = true;
        }
    }
}
