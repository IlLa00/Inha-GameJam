using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerController : BaseCharater
{
    [SerializeField]
    private float WalkForce = 10.0f;
    [SerializeField]
    private float JumpForce = 5.0f;
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

    public event Action<int> UpdateHP;
    
    // Start is called before the first frame update
    void Start()
    {
        this.Rigid2D = GetComponent<Rigidbody2D>();
        base.animator = GetComponent<Animator>();

        this.HP = 10;
        this.Atk = 30;
        CurrentNoiseLevel = NoiseLevel;
        UpdateHP?.Invoke(this.HP);
    }
    
    public float GetNoiseLevel()
    {
        return NoiseLevel;
    }
    public float GetCurrentNoiseLevel()
    {
        return CurrentNoiseLevel;
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
                base.animator.SetFloat("Speed", 0);
                break;
            case State.Walk:
                base.animator.SetFloat("Speed", 0.1f);
                break;
            case State.Run:
                base.animator.SetFloat("Speed", 0.6f);
                break;
            case State.Jump:
                base.animator.SetBool("IsJumping", true);
                break;
            case State.Die:
                base.animator.SetTrigger("Die");
                break;
            case State.Attack:
                base.animator.SetTrigger("Attack");
                break;
            case State.Be_Attacked:
                base.animator.SetTrigger("be_Attacked");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if (Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        bool isRunning = Input.GetKey(KeyCode.LeftShift); //�޸����� Ȯ�� �� ���� force�� �����
        float currentForce = isRunning ? RunForce : WalkForce;
        float currentMaxSpeed = isRunning ? RunMaxSpeed : MaxSpeed;

        this.Rigid2D.AddForce(transform.right * key * currentForce); // �¿� ������
        float velX = Mathf.Abs(this.Rigid2D.velocity.x);

        if (velX > currentMaxSpeed) // �ӵ�����
        {
            this.Rigid2D.velocity = new Vector2(Mathf.Sign(this.Rigid2D.velocity.x) * currentMaxSpeed, this.Rigid2D.velocity.y);
        }

        if (key != 0) // ������ȯ
        {
            float currnetX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(currnetX * key, transform.localScale.y, transform.localScale.z);
        }

        // speed�� ���� �ִϸ��̼��� �ٲ�
        float normalizedSpeed = velX / RunMaxSpeed;
        base.animator.SetFloat("Speed", normalizedSpeed);
    }
   
    void FixedUpdate()
    {
        bool grounded = IsGrounded();
        animator.SetBool("IsJumping", !grounded);  // ���� ������ false, �����̸� true
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded()) //����
        {
            this.Rigid2D.AddForce(Vector2.up * this.JumpForce, ForceMode2D.Impulse);
            ChangeState(State.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //�÷����� ��������?
            ChangeState(State.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            // ��ȣ�ۿ� Ű

            Vector2 origin = Rigid2D.position + new Vector2(10, 0);

            // F�� ������ ���̸����� ��ȣ�ۿ� ������Ʈ�� ������ -> ����
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, 0.75f, LayerMask.GetMask("Interactive_object"));
            Debug.DrawRay(origin, Vector2.right * 0.75f, Color.green);
            
            if(hit)
            {
                GameObject hitObject = hit.collider.gameObject;
                InteractiveObject ob = hit.collider.gameObject.GetComponent<InteractiveObject>();
               
                ob.OnInteractive();
            }

        }
    }
    bool IsGrounded()
    {
        float rayOffsetY = -0.5f; // �ʿ信 ���� ����
        Vector2 origin = Rigid2D.position + new Vector2(0, rayOffsetY);
        float rayLength = 0.2f;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, Vector2.down * rayLength, Color.green);

        return hit.collider != null;
    }
}
