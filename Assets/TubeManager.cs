using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeManager : MonoBehaviour
{
    public int tubeLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    // Incoming Item
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "item_" + tubeLevel)
        {
            Debug.Log("X");
        }
        else
        {
            Debug.Log("O");
        }
        
        Destroy(other.gameObject);
    }
}
