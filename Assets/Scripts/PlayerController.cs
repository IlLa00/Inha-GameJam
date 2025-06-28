using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float WalkForce = 10.0f; 
    float JumpForce = 570.0f;
    float MaxSpeed = 2.0f;
    int Hp = 10;

    Rigidbody2D Rigid2D;
    Animator Animator;

    [SerializeField] Transform groundCheck;
    public event Action<int> UpdateUI;

    bool IsGrounded() // ���� �پ��ִ��� üũ�Լ�
    {
        return Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Ground"));
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Rigid2D = GetComponent<Rigidbody2D>();
        this.Animator = GetComponent<Animator>();
        Animator.speed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("IsGrounded: " + IsGrounded());

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded()) //����
        {
            this.Rigid2D.AddForce(Vector2.up * this.JumpForce);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            //�÷����� ��������?
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            //�޸���
        }

        int key = 0;
        if(Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if(Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        this.Rigid2D.AddForce(transform.right * key * this.WalkForce); // �¿� ������

        float velX = Mathf.Abs(this.Rigid2D.velocity.x);

        if(velX > this.MaxSpeed) // �ӵ�����
        {
            this.Rigid2D.velocity = new Vector2(Mathf.Sign(this.Rigid2D.velocity.x) * this.MaxSpeed, this.Rigid2D.velocity.y);
        }

        if(key != 0) // ������ȯ
        {
            float currnetX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(currnetX * key, transform.localScale.y, transform.localScale.z);
        }


    }
}
