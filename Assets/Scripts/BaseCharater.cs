using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public int HP = 10;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public int Atk;

    protected Animator animator;

    public enum State {Idle, Walk, Attack, Die, Run, Jump, land, Be_Attacked,};
    protected State CurrentState;


    public virtual void Attack(BaseCharater target)
    {
        target.TakeDamage(this.Atk);
        ChangeState(State.Attack);
    }
    public virtual void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage");
        //if (attack_delay <= 0f) return;
        //  attack_delay = 3f;
         HP -= damage;
        
        ChangeState(State.Be_Attacked);

        if(HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        ChangeState(State.Die);
        Debug.Log($"{gameObject.name} Die");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        //DropItem();
        // 3. 일정 시간 후 오브젝트 제거
        //Destroy(gameObject, 1.5f);
        gameObject.SetActive(false);
    }
    public virtual void ChangeState(State newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case State.Idle:
                animator.SetFloat("Speed", 0);
                break;
            case State.Walk:
                animator.SetFloat("Speed", 0.5f);
                break;
            case State.Be_Attacked:
                animator.SetTrigger("be_Attacked");
                break;
            case State.Attack:
                animator.SetTrigger("Attack");
                break;
        }
    }

    public State GetCurrentState()
    {
        return CurrentState;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
