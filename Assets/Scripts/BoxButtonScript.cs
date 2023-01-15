using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxButtonScript : MonoBehaviour
{
    public bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Box collided with " + other.gameObject.name);
        hit = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}