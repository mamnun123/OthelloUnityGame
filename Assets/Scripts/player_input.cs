using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


/*
 *          TO-DO
 * Left-Handedness aspect (later, ASK FOR HELP)
 * Post steering, manipulation concept in moodle
 * Add happy sunflower aspect (Later)
 * Implement manipulation techniques
 * Program the actual game - trash dissapearing when it enters the can
 * Add in controls to networkuserspawner prefab. Check tutorial video.
 * [Organise in order of priority]
 */


// This script is for translating player inputs (using the joysticks or using the keyboard) to player movements in the game
public class TestInput : MonoBehaviour
{
    public InputActionReference actionReference;
    public GameObject headset;
    public float moveForce = 10;
    [SerializeField]
    public float moveSpeed = 10.0f;
    [SerializeField]
    public float moveSpeedDefault = 10.0f;
    private float x = 0;
    private float y = 0;
    private Vector3 newInput;
    private Vector3 newPosition;
    private Vector3 sample = new Vector3(0, 0, 1);
    private float angle;
    public float sensitivity = 2.0f;
    private float rotationX = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checks to see if the player is using a VR headset. If they are, this enables the input settings attached to the script
        if (UnityEngine.XR.XRSettings.enabled == true)
        {
            actionReference.action.Enable();
        }
        moveSpeedDefault = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // This first block runs is if the player is using a VR headset
        if (UnityEngine.XR.XRSettings.enabled == true)
        {

            // It starts by reading in the vector from the player inputs
            Vector2 input = actionReference.action.ReadValue<Vector2>();
            if (input != null)
            {
                // Then it rotates the input vector to the direction that the player is facing.
                angle = Vector3.Angle(sample, headset.transform.forward);
                if (headset.transform.forward.x < 0)
                {
                    angle = -angle;
                }

                // Then it moves the player
                newInput = new Vector3(input.x, 0, input.y);
                newInput = Quaternion.AngleAxis(angle, Vector3.up) * newInput;
                if (15f < transform.position.x + (newInput.x * 0.1f) || transform.position.x + (newInput.x * 0.1f) < -15f)
                {
                    newInput.x = 0;
                } else if (15f < transform.position.z + (newInput.z * 0.1f) || transform.position.z + (newInput.z * 0.1f) < -15f)
                {
                    newInput.z = 0;
                }
                transform.position = new Vector3(transform.position.x + (newInput.x * 0.1f), transform.position.y, transform.position.z + (newInput.z * 0.1f));
            }
        }

        // This second block runs if the player isn't using a VR headset, and they have to control the game with the mouse and keyboard
        else
        {
            // This gets the mouse input
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // This rotates the camera
            rotationX -= mouseY; // Invert Y axis for natural movement
            rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit vertical rotation
            headset.transform.localRotation = Quaternion.Euler(rotationX, headset.transform.localEulerAngles.y + mouseX, 0);

            // This gets the keyboard (wasd or arrow) input for movement by detecting if certain keys are pressed.
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                y = 1;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                x = -1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                y = -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                x = 1;
            }

            // If the control/strg key is held, players can move faster.
            if (Input.GetKey(KeyCode.LeftControl)) {
                moveSpeed = 2 * moveSpeed;
            }

            // Similar to the last block, this rotates the input vector to be facing the direction of the player.
            angle = Vector3.Angle(sample, headset.transform.forward);
            if (headset.transform.forward.x < 0)
            {
                angle = -angle;
            }

            // Then this moves the player.
            // (Apologies for the mess, I'm currently debugging this section)
            newInput = new Vector3(x, 0, y);
            newInput = Quaternion.AngleAxis(angle, Vector3.up) * newInput;
            if (15f < transform.position.x + (newInput.x * 0.1f) || transform.position.x + (newInput.x * 0.1f) < -15f)
            {
                newInput.x = 0;
            }
            else if (15f < transform.position.z + (newInput.z * 0.1f) || transform.position.z + (newInput.z * 0.1f) < -15f)
            {
                newInput.z = 0;
            }
            newPosition = new Vector3(transform.position.x + (newInput.x * moveSpeed), transform.position.y, transform.position.z + (newInput.z * moveSpeed));
            transform.position = newPosition;

            // This resets the values at the end of the frame so that they don't stack 
            x = 0;
            y = 0;
            moveSpeed = moveSpeedDefault;


        }

    }
}
