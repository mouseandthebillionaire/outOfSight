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
        if (other.gameObject.name == "Item_" + tubeLevel + "(Clone)")
        {
            Debug.Log("X");
            GameManager.S.UpdateScore(+1);
        }
        else
        {
            Debug.Log("O");
            GameManager.S.UpdateScore(-1);
        }
        Debug.Log(GameManager.S.score);
        Destroy(other.gameObject);
    }
}
