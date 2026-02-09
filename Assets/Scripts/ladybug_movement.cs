using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;
using Unity.Netcode;

// This script is to move the ladybugs around. It gives them a target to go to and then guides the ladybug to the target.
// If players make contact with the ladybug, they can "collect" them and then they fly around the player
public class ladybug_movement : NetworkBehaviour
{
    public Vector3 destination;
    public float rate = 0.1f;
    public float randTime;
    public Collider target;
    public Collider hitbox;
    private bool playerContact = false;
    private Vector3 newInput;
    private int angle;
    public GameManager GAMEMANAGER;
    private int i = 0;

    // Starts by giving the ladybug a destination to fly to, then gets it to look at the destination
    void Start()
    {
        destination = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0.0f, 3.0f), Random.Range(-15.0f, 15.0f));
        target.gameObject.transform.position = destination;
        transform.LookAt(destination);
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the ladybug if the player hasnt collected it yet.
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
            transform.position = new Vector3(hitbox.gameObject.transform.position.x + newInput.x, hitbox.gameObject.transform.position.y, hitbox.gameObject.transform.position.z + newInput.z);
            angle++;
        }

        GAMEMANAGER.SetTest(new NetworkVariable<int>(i));
        i++;
    }

    // If the ladybug makes contact with the target, changes the placement of the target
    // If the player collects the ladybug, then enables the ladybug to rotate around the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Target")
        {
            target.gameObject.transform.position = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0.0f, 3.0f), Random.Range(-15.0f, 15.0f));
            destination = target.gameObject.transform.position;
        } else if (other.gameObject.name == "Player Hitbox")
        {
            playerContact = true;
            transform.position = new Vector3(hitbox.gameObject.transform.position.x + 5f, hitbox.gameObject.transform.position.y, hitbox.gameObject.transform.position.z);
        }
    }
}