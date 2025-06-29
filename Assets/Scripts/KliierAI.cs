using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class KliierAI : BaseCharater
{
    public float patrolRange = 10f; // 감지 거리 맵만큼으로 바꿔야함
    public float findRange = 5f;
    private Transform player;
    [SerializeField] private LayerMask playerLayer;
    public float chaseSpeed = 4.0f;
    public float waitBeforeChase = 1f;

    public float attackrangeX = 1.0f;
    private Transform attakOrigin;
    private Vector3 startPosition;

    private bool isMovingRight = true;
    private bool isChasing = false;
    private bool isWaiting = false;
    private bool isAttack = false;
    private bool isFullNoiseLevel = false;

    void Start()
    {
        startPosition = transform.position;
        base.Speed = 3f;
        base.Atk = 5;
        base.animator = GetComponent<Animator>();
        base.HP = 1000;
    }

    void Update()
    {
        if (isFullNoiseLevel)
        {
            Chase();
            return;
        }
        DetectPlayer();

        if (isChasing && player != null)
            Chase();
        else if (!isWaiting)
            Patrol();

        if (!isAttack)
            StartCoroutine(AttackPlayer());
    }


    public void ChasePlayer(Transform noiseTransform)
    {
        player = noiseTransform;
        isFullNoiseLevel = true;
        Chase();
    }
    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, findRange, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            if (!isChasing && !isWaiting)
            {
                isWaiting = true;
                player = hit.transform;
                StartCoroutine(WaitAndChase());
            }
        }
        else
        {
            if (isChasing)
            {
                // 플레이어를 놓쳤다면 추적 중단
                isChasing = false;
                player = null;
            }
        }
    }

    System.Collections.IEnumerator WaitAndChase()
    {
        yield return new WaitForSeconds(waitBeforeChase);
        isChasing = true;
        isWaiting = false;
    }

    void Patrol() //순찰
    {
        float direction = isMovingRight ? 1f : -1f;
        Vector3 newPos = transform.position + Vector3.right * direction * Speed * Time.deltaTime;

        // Y 고정
        newPos.y = startPosition.y;
        transform.position = newPos;

        if (Mathf.Abs(newPos.x - startPosition.x) > patrolRange)
            isMovingRight = !isMovingRight;

        FlipSprite(direction);
    }

    void Chase()
    {
        if (player == null) return;

        
        if (isFullNoiseLevel)
        {
            if(transform.position == player.position)
            {
                isFullNoiseLevel = false;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            float direction = player.position.x - transform.position.x;
            FlipSprite(direction);
        }
        else
        {
            Vector3 targetPos = player.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            float direction = targetPos.x - transform.position.x;
            FlipSprite(direction);
        }
                  
    }

    System.Collections.IEnumerator AttackPlayer()
    {
        float Radius = 1.5f;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, Radius);

        if (hit != null && hit.CompareTag("Player"))
        {
            isAttack = true;
            ChangeState(State.Attack);
            base.Attack(hit.GetComponent<PlayerController>());
            yield return new WaitForSecondsRealtime(5f);
            isAttack = false;
        }

    }

    void FlipSprite(float dir)
    {
        if (dir == 0) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(dir) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

}
