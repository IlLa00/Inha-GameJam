using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharater : MonoBehaviour
{
    // Start is called before the first frame update
    public float HP;
    public float Speed;
    public float Atk;

    public enum State {Idle, Move, Attack, Dead}
    public State CurrentState;

    public virtual void Move() { }
    public virtual void Attack() { }
    public virtual void TakeDamage(float damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        CurrentState = State.Dead;
        Debug.Log($"{gameObject.name} Dead");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
