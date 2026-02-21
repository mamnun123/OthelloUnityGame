using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Scoreboard : NetworkBehaviour
{

    public TMP_Text textMesh;
    public GameManager GAMEMANAGER;
    public int player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = GAMEMANAGER.scores[player].ToString();
    }
}
