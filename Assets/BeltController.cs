using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BeltController : MonoBehaviour
{
    private GameObject onOff_btn;
    public  Color[]    onOff_colors; // 0 = on, 1 = off;
    private int        currColor;
    private GameObject grandparent;
    private int        controllerNum;

    private void Start()
    {
        onOff_btn = this.transform.GetChild(0).gameObject;
        grandparent = transform.parent.parent.gameObject;
        controllerNum = grandparent.GetComponent<BeltManager>().beltNum;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "On/Off_" + controllerNum)
                {
                    grandparent.GetComponent<BeltManager>().StartStop();
                    currColor = (currColor + 1) % 2; // Flip between 1 and 0
                    onOff_btn.GetComponent<SpriteRenderer>().color = onOff_colors[currColor];
                }
                if(hit.collider.gameObject.name == "Up_" + controllerNum) grandparent.GetComponent<BeltManager>().SpeedUp();
                if(hit.collider.gameObject.name == "Down_" + controllerNum) grandparent.GetComponent<BeltManager>().SlowDown();
            }
        }
    }
}
