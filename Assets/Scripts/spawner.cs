using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// This script spawns flowers and ladybugs
public class flower_spawn : MonoBehaviour
{

    public GameObject prefab;
    public GameObject ladybug;
    public int maximumFlowers = 50;
    public int maximumLadybugs = 2;
    private Vector3 currVector = Vector3.zero;

    // This runs loops to instantiate flowers and ladybugs at random locations
    void Start()
    {
        for (int i = 0; i < maximumLadybugs; i++)
        {
            Instantiate(ladybug, new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0.0f, 3.0f), Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        }
        for (int i = 0; i < maximumFlowers; i++) {
            currVector = new Vector3(Random.Range(-15.0f, 15.0f), 0.138f, Random.Range(-15.0f, 15.0f));
            Instantiate(prefab, currVector, Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // Need to find a way to do this breadth first
}
