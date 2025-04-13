using UnityEngine;
using AI_FSM;
using System.Collections;

public class Stunned : TaskBase
{
    [SerializeField] protected float _stunnedTime;
    protected Enemy _enemy;
    protected EnemyHealth _health;

    protected override void Start()
    {
        base.Start();     
        _enemy = GetComponent<Enemy>();
        _health = GetComponent<EnemyHealth>();
        _health.OnEnemyAttacked += OnEnemyAttacked;
    }

    private void OnDisable()
    {
        _health.OnEnemyAttacked -= OnEnemyAttacked;
    }

    public void OnEnemyAttacked()
    {
        _enemy.UpdateBehaviour(Enemy.EnemyState.Stunned);
        StartCoroutine(StunnedState());
    }

    IEnumerator StunnedState()
    {
        yield return new WaitForSecondsRealtime(_stunnedTime);
        if ((PlayerMovement.Instance.gameObject.transform.position - transform.position).magnitude > _aiController.GetStoppingDistance())
        {
            _enemy.UpdateBehaviour(Enemy.EnemyState.Chasing);
        }
        else
        {
            _enemy.UpdateBehaviour(Enemy.EnemyState.Attacking);
        }
    }
}
