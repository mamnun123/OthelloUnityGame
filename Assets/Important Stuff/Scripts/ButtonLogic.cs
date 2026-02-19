using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class ButtonLogic : NetworkBehaviour
{

    public GameObject AppleCore;
    public GameObject FishBone;
    public GameObject BrokenPlate;
    public GameObject BeerCan;
    public GameObject Burger;
    public GameObject SodaCan;
    public GameObject VodkaBottle;
    public GameObject Broadcasting;
    public GameObject Ladybug;
    public ButtonInteract StartMenu;
    public GameManager GAMEMANAGER;
    public GameObject toSpawn;

    public void MyFunc()
    {
        if (!IsServer) return;

        for (int i = 0; i < 2; i++)
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
                Instantiate(BrokenPlate, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
            }
            else if (i % 8 == 3)
            {
                Instantiate(BeerCan, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
            }
            else if (i % 8 == 4)
            {
                Instantiate(Burger, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
            }
            else if (i % 8 == 5)
            {
                Instantiate(SodaCan, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
            }
            else if (i % 8 == 6)
            {
                Instantiate(VodkaBottle, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
            }
            else if (i % 8 == 7)
            {
                Instantiate(Broadcasting, new Vector3(Random.Range(-15f, 15f), 5.0f, Random.Range(-15f, 15f)), Random.rotation);
            }
            GAMEMANAGER.trashRemaining.Value += 1;
        }
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(MyCoroutine());
            toSpawn = Instantiate(Ladybug, new Vector3(Random.Range(-15f, 15f), Random.Range(0f, 3f), Random.Range(-15f, 15f)), Quaternion.identity);
            toSpawn.GetComponent<NetworkObject>().Spawn();
        }
        StartMenu.isActive.Value = false;
    }
        

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        yield return null;
    }
}
