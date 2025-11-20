using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class flower_spawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject prefab;
    public int maximumFlowers = 10000000;
    private Vector3 currVector = Vector3.zero;

    void Start()
    {
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
