using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
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

    public void IncreaseCurrentNoiseLevel(float Value)
    {
        CurrentNoiseLevel += Value;

        if(CurrentNoiseLevel >= NoiseLevel)
        {
            // 쵸비상메세지를 센터한테보내고 센터는 이걸 받아서 살인마한테 야 저새기 저깃다 가라 <- 객체지향적
            // 살인마는 그럼 플레이어를 안다 <- 객체지향에 위배가 약간 될수도있음
            // 센터한테 함수를 보내는데 이 함수에는 플레이어 위치를 보내요 센터는 그걸듣고 살인마한테 받은 플레이어위치를 보낸다
            // 나는 응 구현만할꺼야
            
        }
    }

    public void DecreaseCurrentNoiseLevel(float Value)
    {
        CurrentNoiseLevel -= Value;
        CurrentNoiseLevel = Mathf.Clamp01(0f);
    }

    public override void ChangeState(State newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        // 하위 클래스에서 override 가능

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

        bool isRunning = Input.GetKey(KeyCode.LeftShift); //달리는지 확인 후 현재 force를 계산함
        float currentForce = isRunning ? RunForce : WalkForce;
        float currentMaxSpeed = isRunning ? RunMaxSpeed : MaxSpeed;

        this.Rigid2D.AddForce(transform.right * key * currentForce); // 좌우 움직임
        float velX = Mathf.Abs(this.Rigid2D.velocity.x);

        if (velX > currentMaxSpeed) // 속도제한
        {
            this.Rigid2D.velocity = new Vector2(Mathf.Sign(this.Rigid2D.velocity.x) * currentMaxSpeed, this.Rigid2D.velocity.y);
        }

        if (key != 0) // 방향전환
        {
            float currnetX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(currnetX * key, transform.localScale.y, transform.localScale.z);
        }

        // speed에 따라 애니메이션이 바뀜
        float normalizedSpeed = velX / RunMaxSpeed;
        base.animator.SetFloat("Speed", normalizedSpeed);
    }

    void FixedUpdate()
    {
        bool grounded = IsGrounded();
        animator.SetBool("IsJumping", !grounded);  // 땅에 있으면 false, 공중이면 true
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded()) //점프
        {
            this.Rigid2D.AddForce(Vector2.up * this.JumpForce, ForceMode2D.Impulse);
            ChangeState(State.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //플랫포머 내려오기?
            ChangeState(State.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Vector2 origin = Rigid2D.position;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, 5f, LayerMask.GetMask("Interactive_object"));
            Debug.DrawRay(origin, Vector2.right * 3f, Color.green);

            if (hit)
            {
                GameObject hitObject = hit.collider.gameObject;
                InteractiveObject ob = hit.collider.gameObject.GetComponent<InteractiveObject>();

                if (ob.CanInteract()) 
                    ob.OnInteractive();
            }

        }
    }

    bool IsGrounded()
    {
        float rayOffsetY = -0.5f; // 필요에 따라 조정
        Vector2 origin = Rigid2D.position + new Vector2(0, rayOffsetY);
        float rayLength = 0.2f;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, Vector2.down * rayLength, Color.green);

        return hit.collider != null;
    }
}
