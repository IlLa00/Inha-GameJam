using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class KliierAI : BaseCharater
{
    public float patrolRange = 10f; // 맵 범위 만큼 순찰
    public float findRange = 5f;
    private Transform player;
    [SerializeField] private LayerMask playerLayer;
    public float chaseSpeed = 4.0f;
    public float waitBeforeChase = 1f;
    private float patrolYRange = 20.5f;
    private float yUpdateInterval = 2f;
    private float nextYUpdateTime = 0f;
    private float targetY;

    public float attackrangeX = 1.0f;
    private Vector3 startPosition;

    private bool isMovingRight = true;
    private bool isChasing = false;
    private bool isWaiting = false;
    private bool isAttack = false;
    private bool isNoiseEvent = false;

    void Start()
    {
        startPosition = transform.position;
        targetY = startPosition.y;
        nextYUpdateTime = Time.time + yUpdateInterval;

        startPosition = transform.position;
        base.Speed = 3f;
        base.Atk = 5;
        base.animator = GetComponent<Animator>();
        base.HP = 10;

        base.TakeDamage(30);
    }

    void Update()
    {
        if (isNoiseEvent)
            Chase();
        else
        {
            DetectPlayer();

            if (isChasing && player != null)
                Chase();
            else if (!isWaiting)
                Patrol();

            if (!isAttack && isChasing)
                StartCoroutine(AttackPlayer());
        }
            
    }

    public void ChasePlayer(Transform NoisePos)
    {
        player = NoisePos;
        isNoiseEvent = true;
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
                //findPosition.position = transform.position;
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
        if (Time.time >= nextYUpdateTime)
        {
            targetY = startPosition.y + Random.Range(-patrolYRange, patrolYRange);
            nextYUpdateTime = Time.time + yUpdateInterval;
        }

        // Y를 부드럽게 목표까지 이동
        newPos.y = Mathf.Lerp(transform.position.y, targetY, 2f * Time.deltaTime);
        transform.position = newPos;

        if (Mathf.Abs(newPos.x - startPosition.x) > patrolRange)
            isMovingRight = !isMovingRight;

        FlipSprite(direction);
    }

    void Chase()
    {
        if (player == null) return;

        if (isNoiseEvent) // 노이즈 이벤트, 즉 소음게이지가 100일 때 발생.
        {
            if (Vector3.Distance(transform.position, player.position) < 10f)
            {
                isNoiseEvent = false;
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
