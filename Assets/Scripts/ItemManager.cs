using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public  int     direction;
    public  string  instrument;
    private float[] xLocs   = new [] {10f, 9f, 8f, 7f, 6f, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f};
    private int     currLoc = 0;

    public void UpdatePosition()
    {

        if (currLoc == xLocs.Length)
        {
            // It's a "good" item
            if (this.gameObject.name == "Item_0(Clone)")
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/" + instrument);
                GetComponentInParent<BeltManager>().UpdateScore(+1);
            }
            // Or it's a "bad" one
            else
            {
                // Do something to the music?
                GetComponentInParent<BeltManager>().UpdateScore(-1);
            }
            Destroy(this.gameObject);
        }
        else
        { 
            this.transform.localPosition = new Vector3(xLocs[currLoc], this.transform.localPosition.y, 0); 
            currLoc += 1;
            
        }

    }

    // Update is called once per frame
    void OnMouseDown()
    {
        Destroy(this.gameObject);
    }
}
