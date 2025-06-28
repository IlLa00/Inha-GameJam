using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KliierAI : BaseCharater
{
    public float patrolRange = 10f;

    public float findRange = 5f;
     [SerializeField] private LayerMask playerLayer;

    public float chaseSpeed = 3.5f;
    public float waitBeforeChase = 1f;

    private Vector3 startPosition;
    private bool isMovingRight = true;

    private bool isChasing = false;
    private bool isWaiting = false;
    private Transform player;

    void Start()
    {
        startPosition = transform.position;
        base.Speed = 2f;
        base.Atk = 5;
        base.animator = GetComponent<Animator>();
        base.HP = 100;
    }

    void Update()
    {
        DetectPlayer();

        if (isChasing && player != null)
            Chase();
        else if (!isWaiting)
            Patrol();
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

        Vector3 targetPos = player.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        float direction = targetPos.x - transform.position.x;
        FlipSprite(direction);
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
    }
}
