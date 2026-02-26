using NUnit.Framework.Constraints;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


// This script is to move the ladybugs around. It gives them a target to go to and then guides the ladybug to the target.
// If players interact with the ladybug, they can "collect" them and then they fly around the player
public class ladybug_movement : NetworkBehaviour
{
    public Vector3 destination; // The vector of the ladybug's initial spawn, as well as it's 
    private Vector3 newInput; // Vector3 that helps to move the ladybug from place to place
    private int angle; // Angle that helps the ladybug move
    private int oldClientID; // Saves the previous owner of the ladybug, helps redistribute the points during a steal.
    private int count; // Buffer variable that helps with giving the host a modifier
    private int evidence = 1; // Variable that's currently being used in rooting out a bug
    public float rate = 0.1f; // How fast the ladybug's moving
    public bool playerContact = false; // Boolean for if the ladybug has been collected by a player
    private bool firstScoreUpdate = false; // Boolean for if the ladybug has been used to update a modifier
    public SphereCollider target; // Target for a ladybug to travel to
    public Transform player; // The transform of the player that's collected the ladybug
    public GameManager GAMEMANAGER; // The game manager
    public XRSimpleInteractable interactable; // The NetworkList that holds all of the user transforms
    private NetworkObject netObj; // The ladybug's network object


    // Starts by giving the ladybug a destination to fly to, then gets it to look at the destination
    void Start()
    {
        target = GameObject.Find("Target").GetComponent<SphereCollider>(); // The target that the ladybug flies to. Moves when the ladybug interacts with it.
        netObj = GetComponent<NetworkObject>(); // Gets the networkobject component of the ladybug.
        GAMEMANAGER = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>(); // The game manager
        destination = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0.0f, 3.0f), Random.Range(-15.0f, 15.0f)); // The initial spawn point for the ladybug.
        target.gameObject.transform.position = destination; // ^^
        transform.LookAt(destination); // Looks at where it's going
    }

    // Update is called once per frame
    void Update()
    {

        // If the ladybug hasn't been collected yet, it moves around the area, from target to target.
        if (playerContact == false) {
            transform.LookAt(destination);
            transform.position = transform.position + transform.forward * rate;
            transform.LookAt(new Vector3(-destination.x, -destination.y, -destination.z));
        }

        // If the player has collected the ladybug, moves the ladybug around the player.
        else
        {
            if (angle == 180)
            {
                angle = -180;
            }
            newInput = new Vector3(1, 0, 1);
            newInput = Quaternion.AngleAxis(angle, Vector3.up) * newInput;
            transform.position = new Vector3(player.gameObject.transform.position.x + newInput.x, player.gameObject.transform.position.y, player.transform.position.z + newInput.z);
            angle++;
        }

        // "Stealing" function. If the ladybug is interacted with and another player interacts with it, they can "steal" the ladybug.
        if (playerContact == true && firstScoreUpdate == true && oldClientID != (int)netObj.OwnerClientId)
        {
            GAMEMANAGER.SubtractModifier(oldClientID);
            oldClientID = (int)netObj.OwnerClientId;
            GAMEMANAGER.AddModifier(oldClientID);
        }

        // Function that adds the score to the first player that gets it. Different for if the host gets it.
        if (playerContact == true && firstScoreUpdate == false && (int)netObj.OwnerClientId != 0) {

            // This is where the bug is. If a client picks up the ladybug that hasn't been owned by another player, then they don't get the point
            // The first part of the if statement is currently set to an debug.log statement that fires every frame, since the modifier is never added to the client
            if (evidence == GAMEMANAGER.modifiers[(int)netObj.OwnerClientId])
            {
                oldClientID = (int)netObj.OwnerClientId;
                GAMEMANAGER.AddModifier((int)netObj.OwnerClientId);
                Debug.Log("This message is really annoying, wouldn't it be nice if it were posted to the console again?");
            } else
            {
                firstScoreUpdate = true;
            }

        // If the host picks up the ladybug and it hasn't been owned by another player, the program counts for 10 frames before it updates the modifier.
        // This is to ensure that it's connected to the host.
        } else if (playerContact == true && firstScoreUpdate == false && count != 10)
        {
            count++;
        } else if (count == 10 && firstScoreUpdate == false)
        {
            GAMEMANAGER.AddModifier(0);
            firstScoreUpdate = true;
            oldClientID = 0;
        }


    }

    // Returns the ID of the current owner
    public int GetObjectID()
    {
        return (int)netObj.OwnerClientId;
    }


    // If the ladybug makes contact with the target, changes the placement of the target
    // If the player collects the ladybug, then enables the ladybug to rotate around the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerContact = true;
        }
        else if (other.gameObject.name == "Target")
        {
            target.gameObject.transform.position = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0.0f, 3.0f), Random.Range(-15.0f, 15.0f));
            destination = target.gameObject.transform.position;
        }
    }

    public void OnGrabLadybug(SelectEnterEventArgs interactor)
    {
        // Request ownership from server for the grabbing player
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        if (netObj.OwnerClientId != localClientId)
        {
            RequestOwnershipServerRpc(localClientId);
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RequestOwnershipServerRpc(ulong clientId)
    {
        if (netObj.IsSpawned)
        {
            netObj.ChangeOwnership(clientId);

        }
    }
}