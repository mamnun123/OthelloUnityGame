using UnityEngine;
using System.Collections;
using Unity.Netcode;

// Purpose: To spawn trash and ladybugs when the button is pressed.
public class ButtonLogic : NetworkBehaviour
{
    public GameObject AppleCore; // Trash objects
    public GameObject FishBone;
    public GameObject BrokenPlate;
    public GameObject BeerCan;
    public GameObject Burger;
    public GameObject SodaCan;
    public GameObject VodkaBottle;
    public GameObject Broadcasting;
    public GameObject Ladybug; // Ladybug prefab to spawn
    public ButtonInteract StartMenu; // The object containing the button, the title menu, and the scoreboard
    public GameManager GAMEMANAGER; // The game manager
    public GameObject toSpawn; // Buffer object to assist with the spawning of NetoworkObjects

    public void MyFunc()
    {
        if (!IsServer) return;

        // First spawns the trash, according to the value set in the Game Manager
        for (int i = 0; i < GAMEMANAGER.trashCount; i++)
        {
            StartCoroutine(MyCoroutine());
            if (i % 8 == 0)
            {
                toSpawn = Instantiate(AppleCore, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 1)
            {
                toSpawn = Instantiate(FishBone, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 2)
            {
                toSpawn = Instantiate(BrokenPlate, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 3)
            {
                toSpawn = Instantiate(BeerCan, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 4)
            {
                toSpawn = Instantiate(Burger, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 5)
            {
                toSpawn = Instantiate(SodaCan, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 6)
            {
                toSpawn = Instantiate(VodkaBottle, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            else if (i % 8 == 7)
            {
                toSpawn = Instantiate(Broadcasting, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
                toSpawn.GetComponent<NetworkObject>().Spawn();
            }
            GAMEMANAGER.trashRemaining.Value += 1;
        }

        // Then spawns the ladybugs, according to the value in the Game Manager
        for (int i = 0; i < GAMEMANAGER.ladybugCount; i++)
        {
            StartCoroutine(MyCoroutine());
            toSpawn = Instantiate(Ladybug, new Vector3(Random.Range(-15f, 15f), Random.Range(0f, 3f), Random.Range(-15f, 15f)), Quaternion.identity);
            toSpawn.GetComponent<NetworkObject>().Spawn();
        }

        // Re-initialises scores, if a second game is started.
        for (int i = 0; i < 4; i++)
        {
            GAMEMANAGER.scores[i] = 0;
            GAMEMANAGER.modifiers[i] = 1;
        }

        // Removes the title screen and button until the trash is cleaned up.
        StartMenu.isActive.Value = false;
    }
        

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        yield return null;
    }
}
