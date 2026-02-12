using UnityEngine;
using Unity.Netcode;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class trash_data : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string type;
    private string correctSensor;
    private string incorrectSensorOne;
    private string incorrectSensorTwo;
    private string incorrectSensorThree;
    private string incorrectSensorFour;
    private string incorrectSensorFive;
    private string incorrectSensorSix;

    public GameManager GAMEMANAGER;
    
    void Start()
    {
        /*
         * Landfill
         * Paper
         * Bio
         * Yellow
         * Clear Glass
         * Green Glass
         * Brown Glass
         */

        if (type == "Paper")
        {
            type = "Sensor1";
            incorrectSensorOne = "Sensor2";
            incorrectSensorTwo = "Sensor3";
            incorrectSensorThree = "Sensor4";
            incorrectSensorFour = "Sensor5";
            incorrectSensorFive = "Sensor6";
            incorrectSensorSix = "Sensor7";
        }
        else if (type == "Bio")
        {
            type = "Sensor2";
            incorrectSensorOne = "Sensor1";
            incorrectSensorTwo = "Sensor3";
            incorrectSensorThree = "Sensor4";
            incorrectSensorFour = "Sensor5";
            incorrectSensorFive = "Sensor6";
            incorrectSensorSix = "Sensor7";
        }
        else if (type == "Landfill")
        {
            type = "Sensor3";
            incorrectSensorOne = "Sensor2";
            incorrectSensorTwo = "Sensor1";
            incorrectSensorThree = "Sensor4";
            incorrectSensorFour = "Sensor5";
            incorrectSensorFive = "Sensor6";
            incorrectSensorSix = "Sensor7";
        }
        else if (type == "Yellow")
        {
            type = "Sensor4";
            incorrectSensorOne = "Sensor2";
            incorrectSensorTwo = "Sensor3";
            incorrectSensorThree = "Sensor1";
            incorrectSensorFour = "Sensor5";
            incorrectSensorFive = "Sensor6";
            incorrectSensorSix = "Sensor7";
        }
        else if (type == "Clear Glass")
        {
            type = "Sensor5";
            incorrectSensorOne = "Sensor2";
            incorrectSensorTwo = "Sensor3";
            incorrectSensorThree = "Sensor4";
            incorrectSensorFour = "Sensor1";
            incorrectSensorFive = "Sensor6";
            incorrectSensorSix = "Sensor7";
        }
        else if (type == "Brown Glass")
        {
            type = "Sensor6";
            incorrectSensorOne = "Sensor2";
            incorrectSensorTwo = "Sensor3";
            incorrectSensorThree = "Sensor4";
            incorrectSensorFour = "Sensor5";
            incorrectSensorFive = "Sensor1";
            incorrectSensorSix = "Sensor7";
        }
        else if (type == "Green Glass")
        {
            type = "Sensor7";
            incorrectSensorOne = "Sensor2";
            incorrectSensorTwo = "Sensor3";
            incorrectSensorThree = "Sensor4";
            incorrectSensorFour = "Sensor5";
            incorrectSensorFive = "Sensor6";
            incorrectSensorSix = "Sensor1";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == correctSensor)
        {
            if (IsServer)
            {
                Debug.Log("Nice!");
                GAMEMANAGER.AddPoint((int)GetComponent<NetworkObject>().OwnerClientId);
                transform.position = new Vector3(Random.Range(-15, 15), 15, Random.Range(-15, 15));
                //Destroy(this);
            }
        }
        else if (other.gameObject.name == incorrectSensorOne ||
                other.gameObject.name == incorrectSensorTwo ||
                other.gameObject.name == incorrectSensorThree ||
                other.gameObject.name == incorrectSensorFour ||
                other.gameObject.name == incorrectSensorFive ||
                other.gameObject.name == incorrectSensorSix)
        {
            if (IsServer)
            {
                Debug.Log("Wrong bin buddy!");
                transform.position = new Vector3(Random.Range(-15, 15), 15, Random.Range(-15, 15));
            }
        }
    }
}
