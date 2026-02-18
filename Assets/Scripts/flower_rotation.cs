using System.Collections;
using System.Xml;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEditor.PackageManager;
using UnityEngine;
using VRSYS.Core.Networking;

public class flower_rotation : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private UserTransformManager userTransformManager;

    [SerializeField] private float _updateInterval = 1f;
    private Transform closest;

    void Start()
    {
        userTransformManager = FindAnyObjectByType<UserTransformManager>();

        Coroutine rotationCoroutine = StartCoroutine(UpdateRotations());

        StopCoroutine(rotationCoroutine);
    }

    // This code gets the flower to always look at the player
    void Update()
    {
        if (IsServer)
        {
            Debug.Log(NetworkManager.Singleton.ConnectedClients);
            transform.LookAt(GetClosest());
            Debug.Log(GetClosest());
            transform.RotateAround(transform.position, transform.up, 180f);
        }
    }

    private Transform GetClosest()
    {
        Debug.Log("Current Client ID: " + NetworkManager.Singleton.LocalClientId);
        for (int i = 0; i < userTransformManager.userTransforms.Count; i++)
        {
            if (i == 0)
            {
                closest = userTransformManager.userTransforms[0];
            } else
            {
                if (Vector3.Distance(userTransformManager.userTransforms[i].position, gameObject.transform.position) <
                    Vector3.Distance(closest.position, gameObject.transform.position))
                {
                    closest = userTransformManager.userTransforms[i];
                }
            }
        }
        return closest;

    }

    

    private IEnumerator UpdateRotations()
    {
        while (true)
        {
            //...

            yield return new WaitForSeconds(_updateInterval);
        }
    }
}

/*
 * Put networkObject on larger "flower container" object, instead of on all of the children
 */