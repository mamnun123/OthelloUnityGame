using UnityEngine;
using UnityEngine.InputSystem;

public class Vignette : MonoBehaviour
{

    public GameObject vig;
    public InputActionReference toggle;
    private bool isOn = false;
    private bool onReady = false;
    private bool offReady = false;
    private float vignette;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vig.SetActive(false);
        if (UnityEngine.XR.XRSettings.enabled == true)
        {
            toggle.action.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.XR.XRSettings.enabled == true)
        {
            vignette = toggle.action.ReadValue<float>();
            if (vignette > 0.01 && isOn == false && onReady == true)
            {
                vig.SetActive(true);
                isOn = true;
                onReady = false;
            }
            else if (vignette > 0.01 && isOn == true && offReady == true)
            {
                vig.SetActive(false);
                isOn = false;
                offReady = false;
            }
            if (vignette < 0.01 && isOn == true && offReady == false)
            {
                offReady = true;
            } else if (vignette < 0.01 && isOn == false && onReady == false)
            {
                onReady = true;
            }
        } else {
            // just map to a key right now. Consult documentation or ask others later.
            if (Input.GetKeyDown(KeyCode.V) && isOn == false)
            {
                vig.SetActive(true);
                isOn = true;
            }
            else if (Input.GetKeyDown(KeyCode.V) && isOn == true)
            {
                vig.SetActive(false);
                isOn = false;
            }
        }
    }
}
