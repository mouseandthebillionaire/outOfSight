using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeManager : MonoBehaviour
{
    public int    tubeLevel;
    public string tubeInstrument;

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
            GameManager.S.UpdateScore(+1);
            FMODUnity.RuntimeManager.PlayOneShot("event:/" + tubeInstrument);
        }
        else
        {
            GameManager.S.UpdateScore(-1);
        }
        Debug.Log(GameManager.S.score);
        Destroy(other.gameObject);
    }
}
