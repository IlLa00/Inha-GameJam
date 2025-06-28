using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseCharater
{
    float WalkForce = 10.0f; 
    float JumpForce = 10.0f;
    float MaxSpeed = 2.0f;
    float NoiseLevel = 100;
    float CurrentNoiseLevel;
    int cureentHP;
    
    Rigidbody2D Rigid2D;
    Animator Animator;

    [SerializeField] Transform groundCheck;

    public event Action<int> UpdateHP;

    

    // Start is called before the first frame update
    void Start()
    {
        this.Rigid2D = GetComponent<Rigidbody2D>();
        this.Animator = GetComponent<Animator>();
        Animator.speed = 0.3f;

        this.HP = 10;
        this.Atk = 30;
        CurrentNoiseLevel = NoiseLevel;
        UpdateHP?.Invoke(cureentHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) //점프
        {
            this.Rigid2D.AddForce(Vector2.up * this.JumpForce, ForceMode2D.Impulse);
            Animator.SetBool("IsJumping", true);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            //플랫포머 내려오기?
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            //달리기
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            // 상호작용 키
        }

        int key = 0;
        if(Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if(Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        this.Rigid2D.AddForce(transform.right * key * this.WalkForce); // 좌우 움직임

        float velX = Mathf.Abs(this.Rigid2D.velocity.x);

        if(velX > this.MaxSpeed) // 속도제한
        {
            this.Rigid2D.velocity = new Vector2(Mathf.Sign(this.Rigid2D.velocity.x) * this.MaxSpeed, this.Rigid2D.velocity.y);
        }

        if(key != 0) // 방향전환
        {
            float currnetX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(currnetX * key, transform.localScale.y, transform.localScale.z);
        }

 
    }

    private void FixedUpdate()
    {
        //Landin Ploatform
        if(Rigid2D.velocity.y < 0) //내려갈때만 스캔
        {
            Debug.DrawRay(Rigid2D.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(Rigid2D.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    Animator.SetBool("IsJumping", false);
                }
            }
        }
    }
}
