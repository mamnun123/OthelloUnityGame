using UnityEngine;

public class quick_rotation : MonoBehaviour
{
    public GameObject hand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.XR.XRSettings.enabled == false)
        {
            if (Input.GetKey(KeyCode.I))
            {
                // X negative
            }
            if (Input.GetKey(KeyCode.J))
            {
                // Y negative
            }
            if (Input.GetKey(KeyCode.K))
            {
                // X positive
            }
            if (Input.GetKey(KeyCode.L))
            {
                // Y positive
            }
        }
    }
}
