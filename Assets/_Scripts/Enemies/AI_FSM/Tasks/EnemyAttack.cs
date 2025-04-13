using UnityEngine;
using AI_FSM;

public abstract class EnemyAttack : TaskBase
{
    [SerializeField, Range(0f,100f)] protected float _damage;
    [SerializeField, Range(0f, 100f)] protected float _range;
    [SerializeField, Range(0f, 100f)] protected float _attackArea;

    public abstract void Attack();
}
