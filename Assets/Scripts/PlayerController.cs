using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseCharater
{
    [SerializeField]
    private float WalkForce = 10.0f;
    [SerializeField]
    private float JumpForce = 10.0f;
    [SerializeField]
    private float MaxSpeed = 2.0f;
    [SerializeField]
    private float NoiseLevel = 100;
    [SerializeField]
    private float CurrentNoiseLevel = 0;
    [SerializeField]
    private float RunForce = 20.0f;
    [SerializeField]
    private float RunMaxSpeed = 4.0f;

    Rigidbody2D Rigid2D;
    Animator Animator;

    public event Action<int> UpdateHP;

    // Start is called before the first frame update
    void Start()
    {
        this.Rigid2D = GetComponent<Rigidbody2D>();
        this.Animator = GetComponent<Animator>();

        this.HP = 10;
        this.Atk = 30;
        CurrentNoiseLevel = NoiseLevel;
        UpdateHP?.Invoke(this.HP);
    }

    public override void ChangeState(State newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        // ���� Ŭ�������� override ����

        switch (newState)
        {
            case State.Idle:
                Animator.SetFloat("Speed", 0);
                break;
            case State.Walk:
                Animator.SetFloat("Speed", 0.5f);
                break;
            case State.Run:
                Animator.SetFloat("Speed", 1f);
                break;
            case State.Jump:
                Animator.SetBool("Jump", true);
                break;
            case State.Die:
                Animator.SetBool("IsDead", true);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleInput();

        int key = 0;
        if(Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if(Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentForce = isRunning ? RunForce : WalkForce;
        float currentMaxSpeed = isRunning ? RunMaxSpeed : MaxSpeed;

        this.Rigid2D.AddForce(transform.right * key * currentForce); // �¿� ������
        float velX = Mathf.Abs(this.Rigid2D.velocity.x);

        if(velX > currentMaxSpeed) // �ӵ�����
        {
            this.Rigid2D.velocity = new Vector2(Mathf.Sign(this.Rigid2D.velocity.x) * currentMaxSpeed, this.Rigid2D.velocity.y);
        }

        if(key != 0) // ������ȯ
        {
            float currnetX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(currnetX * key, transform.localScale.y, transform.localScale.z);
        }

        float normalizedSpeed = velX / RunMaxSpeed;
        Animator.SetFloat("Speed", normalizedSpeed);
    }

    private void FixedUpdate()
    {
        // �ٴڿ� ����ִ� �������� üũ
        if (Rigid2D.velocity.y <= 0) //���������� ��ĵ
        {
            Debug.DrawRay(Rigid2D.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(Rigid2D.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.6f)
                {
                    Animator.SetBool("IsJumping", false);
                }
            }
        }

    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !Animator.GetBool("IsJumping")) //����
        {
            this.Rigid2D.AddForce(Vector2.up * this.JumpForce, ForceMode2D.Impulse);
            Animator.SetBool("IsJumping", true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //�÷����� ��������?
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            // ��ȣ�ۿ� Ű
        }
    }
}
