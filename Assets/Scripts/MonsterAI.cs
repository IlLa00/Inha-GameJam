using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class MonsterAI : BaseCharater
{
    // Start is called before the first frame update

    public Transform GroundCheck;
    private Rigidbody2D Rigid2D;

    [SerializeField] LayerMask GroundLayer;

    public float GroundCheckDistance = 1.2f;
    private bool isMovingRight = true;

    [SerializeField] private float offsetX = 0.5f;

    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private float RunSpeed = 2f;
    [SerializeField] private float attackrangeX = 1.0f;
    [SerializeField] private Transform attakOrigin;
    [SerializeField] private Vector2 boxsize = new Vector2(0.8f, 1f);
    [SerializeField] private LayerMask Player;

    private Transform player;
    private bool isPlayerDetected = false;
    private bool isChasing = false;
    private bool isWaiting = false;
    private bool isAttack = false;


    public event Action OnDeath;
    void Start()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        base.animator = GetComponent<Animator>();
        base.Speed = 2f;
        base.HP = 1;
        base.Atk = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Sencing(); // 감지

        if (isChasing && player != null)
            HandlChase(); // 감지되면 추격
        else if(!isWaiting)
            Patrol(); //순찰

        if(isAttack == false)
            StartCoroutine(IsOnAttackPlayer());
    }
    void Patrol() //raycast로 땅 position 
    {
        base.ChangeState(State.Walk);
        Vector2 groundCheckPos = new Vector3(transform.position.x + (isMovingRight ? offsetX : -offsetX), transform.position.y);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos, Vector2.down, GroundCheckDistance, GroundLayer); //레이를 발밑으로 쏜다
        if(!groundInfo.collider)
        {
            flip();
        }

        Rigid2D.velocity = new Vector2((isMovingRight ? 1 : -1) * Speed, Rigid2D.velocity.y);
    }
    void flip() // 방향 바꾸는 함수
    {
        isMovingRight = !isMovingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void Sencing()
    {
        if (isPlayerDetected)
            return;

        Vector2 boxSize = new Vector2(8f, 3f); // 가로 X, 세로 Y
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(0f, -0.1f);

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, PlayerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            player = hit.transform;
            isPlayerDetected = true;

            float dirToPlayer = player.position.x - transform.position.x;
            if ((isMovingRight && dirToPlayer < 0) || (!isMovingRight && dirToPlayer > 0))
            {
                flip();
            }

            if (!isWaiting)
            {
                StartCoroutine(WaitAndChase());
            }
        }
        else
        {
            if(isChasing)
            {
                isChasing = false;
                player = null;
            }
        }
    }
    void HandlChase()
    {
        if (!isChasing || player == null)
            return;
        base.ChangeState(State.Walk);
        float dirToPlayer = player.position.x - transform.position.x;
        if ((isMovingRight && dirToPlayer < 0) || (!isMovingRight && dirToPlayer > 0))
        {
            flip();
        }
        transform.position = Vector2.MoveTowards(transform.position, player.position, RunSpeed * Time.deltaTime);
    }
    //void IsOnAttackPlayer()
    //{
    //    Vector2 offset = new Vector2(transform.localScale.x > 0 ? attackrangeX : -attackrangeX, 0);
    //    Vector2 attackPoint = (Vector2)attakOrigin.position + offset;

    //    Collider2D hit = Physics2D.OverlapBox(attackPoint, boxsize, 0f, Player);
    //    if (hit != null && hit.CompareTag("Player"))
    //    {
    //        ChangeState(State.Attack);
    //        hit.GetComponent<PlayerController>().TakeDamage(base.Atk);
    //        StartCoroutine(DelayAttack());
    //    }
    //}
    System.Collections.IEnumerator WaitAndChase()
    {
        isWaiting = true;
        Rigid2D.velocity = Vector2.zero;
        base.ChangeState(State.Idle);
        yield return new WaitForSecondsRealtime(3f); // 3초 대기

        isChasing = true;
    }

    System.Collections.IEnumerator IsOnAttackPlayer()
    {
        Vector2 offset = new Vector2(transform.localScale.x > 0 ? attackrangeX : -attackrangeX, 0);
        Vector2 attackPoint = (Vector2)attakOrigin.position + offset;

        Collider2D hit = Physics2D.OverlapBox(attackPoint, boxsize, 0f, Player);

        if (hit != null && hit.CompareTag("Player") )
        {
            isAttack = true;
            ChangeState(State.Attack);
            base.Attack(hit.GetComponent<PlayerController>());
            yield return new WaitForSecondsRealtime(3f);
            isAttack = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 박스 감지 시
        Vector2 boxSize = new Vector2(8f, 3f);
        Vector2 boxCenter = transform.position + new Vector3(0f, -0.1f);
        Gizmos.DrawWireCube(boxCenter, boxSize);

    }

    protected override void Die()
    {
        OnDeath?.Invoke();
        base.Die();
    }

    public void ResetMonster()
    {
        isPlayerDetected = false;
        isChasing = false;
        isWaiting = false;
        isAttack = false;

        player = null;

        // 필요한 경우 체력 초기화
        base.HP = 1;
        this.enabled = true;

        // Rigidbody와 콜라이더가 꺼져 있었을 경우 대비
        GetComponent<Rigidbody2D>().simulated = true;

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        if (animator != null)
            animator.Rebind();  // 애니메이터 초기화
    }
}
