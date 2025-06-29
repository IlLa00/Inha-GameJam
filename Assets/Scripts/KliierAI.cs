using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class KliierAI : BaseCharater
{
    public float patrolRange = 10f;
    public float findRange = 5f;
    private Transform player;
    [SerializeField] private LayerMask playerLayer;
    public float chaseSpeed = 3.5f;
    public float waitBeforeChase = 1f;

    public float attackrangeX = 1.0f;
    public float attackrangeY = 1.0f;
    private Transform attakOrigin;
    private Vector2 boxsize = new Vector2(0.8f, 1f);
    private Vector3 startPosition;

    private bool isMovingRight = true;
    private bool isChasing = false;
    private bool isWaiting = false;
    private bool isAttack = false;
    private bool isFullNoiseLevel = false;

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
                // �÷��̾ ���ƴٸ� ���� �ߴ�
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
    System.Collections.IEnumerator AttackPlayer()
    {
        Vector2 offset = new Vector2(transform.localScale.x > 0 ? attackrangeX : -attackrangeX, 0);
        yield return new WaitForSeconds(2f);
    }

    void Patrol() //����
    {
        float direction = isMovingRight ? 1f : -1f;
        Vector3 newPos = transform.position + Vector3.right * direction * Speed * Time.deltaTime;

        // Y ����
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
