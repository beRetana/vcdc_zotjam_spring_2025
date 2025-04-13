using UnityEngine;
using AI_FSM;

public abstract class EnemyAttack : TaskBase
{
    [SerializeField] protected float _damage;
    [SerializeField] protected float _range;

    public abstract void Attack();
}
