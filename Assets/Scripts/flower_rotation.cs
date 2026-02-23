using System.Collections;
using System.Xml;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEditor.PackageManager;
using UnityEngine;
using VRSYS.Core.Networking;

// Purpose: To calculate the direction that the flower is rotating. Flowers should rotate to face the player that's closest to them.
public class flower_rotation : NetworkBehaviour
{
    private UserTransformManager userTransformManager; // Variable for the data from the UserTransformManager
    [SerializeField] private float _updateInterval = 1f; // Interval at which the flowers update their rotation status.
    private Transform closest; // Transform for the closest player

    void Start()
    {
        userTransformManager = FindAnyObjectByType<UserTransformManager>(); // Assigns variable
        Coroutine rotationCoroutine = StartCoroutine(UpdateRotations());
        StopCoroutine(rotationCoroutine);
    }

    // This code gets the flower to always look at the player
    void Update()
    {
        if (IsServer)
        {
            transform.LookAt(GetClosest()); // Finds and looks AWAY FROM the closest player
            transform.RotateAround(transform.position, transform.up, 180f); // Rotates the flower 180 degrees due to the posiutioning of the model.
        }
    }

    private Transform GetClosest()
    {
        // Compares distance to all player transforms to find who's closest.
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