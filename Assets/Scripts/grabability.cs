using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class grabability : MonoBehaviour
{

    public GameObject trashSample;
    public XRRayInteractor rightRay;

    public float distance = 0;
    public float maxDistance = 100f;
    public float heldDistance = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rightRay.TryGetCurrent3DRaycastHit(out RaycastHit rightHit))
        {
            if (rightHit.collider.name == "TestTrash")
            {
                // Turn beam yellow
                distance = Vector3.Distance(transform.position, trashSample.transform.position);
                if (Input.GetKeyDown(KeyCode.LeftShift) && distance <= maxDistance)
                {
                    heldDistance = distance;
                }
                if (Input.GetKey(KeyCode.LeftShift) && distance <= maxDistance)
                {
                    Debug.Log(transform.forward);
                    trashSample.transform.position = transform.forward * heldDistance;
                    // need a way to disable this if it's colliding with the floor
                }
                if (Input.GetKeyUp(KeyCode.LeftShift) && heldDistance != 0) 
                {
                    heldDistance = 0;
                }
            }
        }
    }
}
