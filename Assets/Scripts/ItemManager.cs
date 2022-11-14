using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public  int     direction; // 0=left | 1=right
    public  string  instrument;
    private string  playEvent;
    private float[] xLocs   = new [] {10f, 9f, 8f, 7f, 6f, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f};
    private int     currLoc = 0;

    public void UpdatePosition()
    {
        // We're at the end of the line
        if (currLoc == xLocs.Length)
        {
            // It's a "good" item
            if (this.gameObject.name == "Item_0(Clone)")
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/" + instrument);
                playEvent = "event:/" + instrument;
                GetComponentInParent<BeltManager>().UpdateScore(+1);
            }
            // Or it's a "bad" one
            else
            {
                // Do something to the music?
                playEvent = "event:/Boosh";
                GetComponentInParent<BeltManager>().ResetScore();
            }
            // Update the effect based on the Score
            string instrumentEffect = GetComponentInParent<BeltManager>().instrumentEffect;
            float effectAmmount = GetComponentInParent<BeltManager>().score / 40f;
            if (effectAmmount > 1) effectAmmount = 1;
            AudioHelper.PlayOneShotWithParameters(playEvent, this.transform.position, (instrumentEffect, effectAmmount));
            Destroy(this.gameObject);
        }
        // Or we're still going
        else
        { 
            this.transform.localPosition = new Vector3(xLocs[currLoc], this.transform.localPosition.y, 0);
            int directionMod;
            if (direction == 0) directionMod = 1;
            else directionMod = -1;
            GetComponent<SpriteRenderer>().sortingOrder = 18 - (currLoc * directionMod);
            currLoc += 1;
            
        }

    }

    // Update is called once per frame
    void OnMouseDown()
    {
        Destroy(this.gameObject);
    }
}
