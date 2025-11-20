using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    public InputActionReference actionReference; 
    public float moveForce = 100f;
    private Vector3 currPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionReference.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = actionReference.action.ReadValue<Vector2>();
        if (input != null)
        {
            currPos = transform.position;
            transform.position = new Vector3(currPos.x + (input.x*0.1f), currPos.y, currPos.z + (input.y*0.1f));
        }
    }
}
