using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Grabability4Desktop : MonoBehaviour
{
    public GameObject testTrash;
    public XRRayInteractor rightRay;
    private bool isGrabbing = false;
    // Eventually will have to define a list for all of the garbage items

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing == true && Input.GetKeyDown(KeyCode.E))
        {
            isGrabbing = false;
        }
        if (UnityEngine.XR.XRSettings.enabled == false && rightRay.TryGetCurrent3DRaycastHit(out RaycastHit rightHit))
        {
            if (rightHit.collider.name == "TestTrash" && Input.GetKeyDown(KeyCode.E))
            {
                isGrabbing = !isGrabbing;
            }
        }
        if (UnityEngine.XR.XRSettings.enabled == false && isGrabbing == true)
        {
            testTrash.transform.position = transform.forward * 5;
        }
        if (isGrabbing == true)
        {
            Debug.Log("Is Grabbing");
        }
    }
}
