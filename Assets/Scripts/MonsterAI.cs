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
    [SerializeField] private float FindRange = 5f;
    [SerializeField] private float RunSpeed = 2f;

    private Transform player;
    private bool isPlayerDetected = false;
    private bool isChasing = false;
    private bool isWaiting = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sencing();
        HandlChase();
        Patrol();
    }
    void Patrol() //raycast∑Œ ∂• position 
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

        Collider2D hit = Physics2D.OverlapCircle(transform.position, FindRange, PlayerLayer);
        if(hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("33");
            player = hit.transform;
            isPlayerDetected = true;

            if(!isWaiting)
            {
                StartCoroutine(WaitThenChase());
            }
        }
        else { isPlayerDetected = false; }
    }
    void HandlChase()
    {
        if (!isChasing || player == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, player.position, RunSpeed * Time.deltaTime);
    }
    IEnumerator WaitThenChase()
    {
        isWaiting = true;

        yield return new WaitForSecondsRealtime(3f); // 3√  ¥Î±‚

        isChasing = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FindRange);
    }
}
