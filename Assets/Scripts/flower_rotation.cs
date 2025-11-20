using UnityEngine;

public class flower_rotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            transform.LookAt(player);
            transform.RotateAround(transform.position, transform.up, 180f);
        }
    }
}
