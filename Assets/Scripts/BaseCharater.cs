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

    public virtual void Move() { }
    public virtual void Attack(BaseCharater target)
    {
        target.TakeDamage(this.Atk);
        ChangeState(State.Attack);
    }
    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        ChangeState(State.Be_Attacked);

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
        // ���� Ŭ�������� override ����

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
