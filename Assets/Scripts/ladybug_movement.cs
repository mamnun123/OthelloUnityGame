using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// This script is to move the ladybugs around. It gives them a target to go to and then guides the ladybug to the target.
// If players make contact with the ladybug, they can "collect" them and then they fly around the player
public class ladybug_movement : NetworkBehaviour
{
    public Vector3 destination;
    public float rate = 0.1f;
    public float randTime;
    public SphereCollider target;
    public Collider hitbox;
    public bool playerContact = false;
    private bool thisOne = false;
    public Transform player;
    private Vector3 newInput;
    private GameObject save;
    private int angle;
    public GameManager GAMEMANAGER;
    private int i = 0;
    public XRSimpleInteractable interactable;
    private NetworkObject netObj;
    private bool firstScoreUpdate = false;
    private int oldClientID;
    private int count;


    // Starts by giving the ladybug a destination to fly to, then gets it to look at the destination
    void Start()
    {
        target = GameObject.Find("Target").GetComponent<SphereCollider>();
        GAMEMANAGER = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>();
        destination = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0.0f, 3.0f), Random.Range(-15.0f, 15.0f));
        target.gameObject.transform.position = destination;
        transform.LookAt(destination);
        netObj = GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerContact == false) {
            transform.LookAt(destination);
            transform.position = transform.position + transform.forward * rate;
            transform.LookAt(new Vector3(-destination.x, -destination.y, -destination.z));
        }

        // If the player has collected the ladybug, moves the ladybug around the player 
        // (Currently does not work)
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

        // "Stealing" function
        if (playerContact == true && firstScoreUpdate == true && oldClientID != (int)netObj.OwnerClientId)
        {
            GAMEMANAGER.SubtractModifier(oldClientID);
            oldClientID = (int)netObj.OwnerClientId;
            GAMEMANAGER.AddModifier(oldClientID);
            Debug.Log("Stolen!");
        }

        // Function that adds the score to the first player that gets it. Different for if the host gets it.
        if (playerContact == true && firstScoreUpdate == false && (int)netObj.OwnerClientId != 0) {
            GAMEMANAGER.AddModifier((int)netObj.OwnerClientId);
            firstScoreUpdate = true;
            oldClientID = (int)netObj.OwnerClientId;
            Debug.Log("Non-client first count");
        } else if (playerContact == true && firstScoreUpdate == false && count != 10)
        {
            count++;
        } else if (count == 10 && firstScoreUpdate == false)
        {
            GAMEMANAGER.AddModifier(0);
            firstScoreUpdate = true;
            oldClientID = 0;
            Debug.Log("Client first count");
        }


    }

    public int GetObjectID()
    {
        Debug.Log("LADYBUG OWNER: " + (int)netObj.OwnerClientId);
        return (int)netObj.OwnerClientId;
    }

    // If the ladybug makes contact with the target, changes the placement of the target
    // If the player collects the ladybug, then enables the ladybug to rotate around the player
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Somewhat Contact Aquired");
        if (other.CompareTag("Player"))
        {
            playerContact = true;
            Debug.Log("Contact Aquired");
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
        Debug.Log("Laser1");
        if (netObj.IsSpawned)
        {
            Debug.Log("Laser2");
            netObj.ChangeOwnership(clientId);

        }
    }
}