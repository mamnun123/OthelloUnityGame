using UnityEngine;
using Unity.Netcode;

public class PlayerSearch : NetworkBehaviour
{

    public NetworkVariable<string> player1 = null;
    public NetworkVariable<string> player2 = null;
    string name;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (player1 == null || player2 == null)
        {
            for (int i = 1; i < 1000; i++)
            {
                name = "Player_" + i.ToString();
                if (GameObject.Find(name) != null && player1 == null)
                {
                    Debug.Log("Found one: " + name);
                    player1 = new NetworkVariable<string>(name);
                }
                else if (GameObject.Find(name) != null && player1 != null && player2 == null)
                {
                    Debug.Log("Found two: " + name);
                    player2 = new NetworkVariable<string>(name);
                }
            }
        }
    }
}
