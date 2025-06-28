using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharater : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 10;
    public float Speed;
    public int Atk;

    Animator animator;

    public enum State {Idle, Walk, Attack, Die, Run, Jump, land, Be_Attacked,};
    protected State CurrentState;

    public virtual void Move() { }
    public virtual void Attack() { }
    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        CurrentState = State.Die;
        Debug.Log($"{gameObject.name} Die");
    }
    public virtual void ChangeState(State newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        // 하위 클래스에서 override 가능

        switch (newState)
        {
            case State.Idle:
                animator.SetFloat("Speed", 0);
                break;
            case State.Walk:
                animator.SetFloat("Speed", 0.5f);
                break;
            case State.Die:
                animator.SetBool("IsDead", true);
                break;
            //case State.Attack:
              
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
