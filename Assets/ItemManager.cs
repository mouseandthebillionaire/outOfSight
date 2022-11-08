using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public float itemSpeed;
    public int   direction;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.left * itemSpeed);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
