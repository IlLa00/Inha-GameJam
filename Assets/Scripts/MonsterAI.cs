using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterAI : BaseCharater
{
    // Start is called before the first frame update

    public Transform GroundCheck;
    private Rigidbody2D Rigid2D;

    [SerializeField] LayerMask GroundLayer;

    public float GroundCheckDistance = 1f;
    private bool isMovingRight = true;


    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private float FindRange = 3f;
    [SerializeField] private float RunSpeed = 2f;

    private Transform player;
    private bool isPlayerDetected = false;
    private bool isChasing = false;
    private bool isWaiting = false;

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
            HandlChase();
        else if(!isWaiting)
            Patrol(); //순찰

    }
    void Patrol() //raycast로 땅 position 
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(GroundCheck.position, Vector2.down, GroundCheckDistance, GroundLayer);
        if(!groundInfo.collider)
        {
            flip();
        }

        Rigid2D.velocity = new Vector2((isMovingRight ? 1 : -1) * Speed, Rigid2D.velocity.y);
    }
    void flip()
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

            if(!isWaiting)
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

        transform.position = Vector2.MoveTowards(transform.position, player.position, RunSpeed * Time.deltaTime);
    }
    System.Collections.IEnumerator WaitAndChase()
    {
        isWaiting = true;
        Rigid2D.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(3f); // 3초 대기

        isChasing = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 박스 감지 시
        Vector2 boxSize = new Vector2(8f, 3f);
        Vector2 boxCenter = transform.position + new Vector3(0f, -0.1f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
