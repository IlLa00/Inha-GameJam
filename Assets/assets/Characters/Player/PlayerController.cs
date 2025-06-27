using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float WalkForce = 30.0f;
    float JumpForce = 680.0f;

    Rigidbody2D rigid2D;
    
    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().speed = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.rigid2D.AddForce(transform.up * this.JumpForce);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {

        }

        int key = 0;
        if(Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if(Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        this.rigid2D.AddForce(transform.right * key * this.WalkForce);
    }
}
