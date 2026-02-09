using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Unity.Netcode;
using UnityEngine.Serialization;
using TMPro;

public class Teleportation : MonoBehaviour
{

    public GameObject player;
    public GameObject hitPoint;
    public GameObject previewAvatar;
    public InputActionProperty leftTrigger;
    public InputActionProperty rightTrigger;
    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;
    private bool canTeleport = false;
    private bool cueTP = false;
    private bool hold = false;
    private float input = 0;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftTrigger.action.Enable();
        hitPoint.transform.position = new Vector3(0, -100f, 0);
        previewAvatar.transform.position = new Vector3(0, -100f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        input = leftTrigger.action.ReadValue<float>();
        EnableTeleport();
        GetHitPoint();
        GoTeleport();
    }

    void EnableTeleport()
    {
        if (input > 0.001 || Input.GetKeyDown(KeyCode.T))
        {
            canTeleport = true;
        } else
        {
            canTeleport = false;
        }

        if (UnityEngine.XR.XRSettings.enabled == true && input < 0.001 && hold == true)
        {
            hitPoint.transform.position = new Vector3(0, -100f, 0);
            previewAvatar.transform.position = new Vector3(0, -100f, 0);
            hold = false;
        }
    }

    void GetHitPoint()
    {
        if (leftRay.TryGetCurrent3DRaycastHit(out RaycastHit leftHit) && canTeleport == true)
        {
            Vector3 intersectionPoint = leftHit.point;
            if (leftHit.collider.name == "Plane" &&
                -15f <= leftHit.point.x && leftHit.point.x <= 15f &&
                -15f <= leftHit.point.z && leftHit.point.z <= 15f)
            {
                if (cueTP)
                {
                    hitPoint.transform.position = new Vector3(intersectionPoint.x, 0, intersectionPoint.z);
                    previewAvatar.transform.LookAt(new Vector3(hitPoint.transform.position.x, 1.2f, hitPoint.transform.position.z));
                    // Rotate thingy here?
                } else if (hold)
                {
                    hitPoint.transform.position = intersectionPoint;
                    previewAvatar.transform.position = intersectionPoint + new Vector3(0, 1.2f, 0);
                }
            }
            if (leftHit.collider.name == "Cylinder" ||
                leftHit.collider.name == "Cylinder (1)" ||
                leftHit.collider.name == "Cylinder (2)" ||
                leftHit.collider.name == "Cylinder (3)" ||
                leftHit.collider.name == "Cylinder (4)" ||
                leftHit.collider.name == "Cylinder (5)" ||
                leftHit.collider.name == "Cylinder (6)")
            {
                Vector3 newIntersectionPoint = new Vector3(intersectionPoint.x, 0, intersectionPoint.z);
                if (cueTP)
                {
                    hitPoint.transform.position = new Vector3(intersectionPoint.x, 0, intersectionPoint.z);
                    previewAvatar.transform.LookAt(new Vector3(hitPoint.transform.position.x, 1.2f, hitPoint.transform.position.z));
                    // Rotate thingy here?
                }
                else if (hold)
                {
                    hitPoint.transform.position = newIntersectionPoint;
                    previewAvatar.transform.position = newIntersectionPoint + new Vector3(0, 1.2f, 0);
                }
                // How to ignore a object when doing raycast?
            }
        } 
    }

    void GoTeleport()
    {
        if (canTeleport == true) 
        {
            if (UnityEngine.XR.XRSettings.enabled == false && Input.GetKeyDown(KeyCode.Space))
            {
                player.transform.position = new Vector3(hitPoint.transform.position.x, 0.323f, hitPoint.transform.position.z);
            } else if (UnityEngine.XR.XRSettings.enabled == true && input > 0.001)
            {
                Debug.Log(input);
                Debug.Log(hold);
                // Preparing to teleport. Can still back out.
                if (input < 0.001 && hold == true) {
                    hitPoint.transform.position = new Vector3(0, -100f, 0);
                    previewAvatar.transform.position = new Vector3(0, -100f, 0);
                    hold = false;
                } else if (input < 1 && input > 0.001 && cueTP == false)
                {
                    hold = true;
                    // Max teleport - upon relese, teleport will occur.
                }
                else if (input == 1)
                {
                    hold = true;
                    cueTP = true;
                    // Teleportation action!
                }
                else if (input < 1 && input > 0.001 && cueTP == true)
                {
                    if (-15f <= previewAvatar.transform.position.x && previewAvatar.transform.position.x <= 15f &&
                        -15f <= previewAvatar.transform.position.z && previewAvatar.transform.position.z <= 15f)
                    {
                        player.transform.position = new Vector3(previewAvatar.transform.position.x, 0.323f, previewAvatar.transform.position.z);
                        player.transform.LookAt(new Vector3(hitPoint.transform.position.x, 0.323f, hitPoint.transform.position.z));
                    }
                    hold = false;
                    cueTP = false;
                    hitPoint.transform.position = new Vector3(0, -100f, 0);
                    previewAvatar.transform.position = new Vector3(0, -100f, 0);
                }
            }
        }
        // edit all ts to make it look better, and to incorporate the delay into the keyboard playstyle
    }
}
