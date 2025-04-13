using AI_FSM;
using UnityEngine;

public class Enemy : AIActor
{
    protected ChasePlayer _chasePlayer;
    protected EnemyAttack _enemyAttack;
    protected Stunned _stunned;
    protected GameObject _player;
    protected EnemyState _enemyState;
    protected AIController _controller;

    public enum EnemyState
    {
        Chasing,
        Attacking,
        Stunned,
    }

    protected void Start()
    {
        _enemyState = EnemyState.Chasing;
        _controller = GetComponent<AIController>();
        _chasePlayer = GetComponent<ChasePlayer>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _stunned = GetComponent<Stunned>();

        _chasePlayer.Enable();

        _player = PlayerMovement.Instance.gameObject;
        _controller.SetRotationActive(false);
    }

    public virtual void UpdateBehaviour(EnemyState newEnemyState)
    {
        if (newEnemyState != _enemyState)
        {
            _enemyState = newEnemyState;
            
            AbortBehaviors();
        }
        TransitionOfBehaviors();
    }

    public override void TransitionOfBehaviors()
    {
        switch (_enemyState)
        {
            case EnemyState.Chasing:
                _chasePlayer.Enable();
                break;
            case EnemyState.Attacking:
                _enemyAttack.Enable();
                break;
            case EnemyState.Stunned:
                break;
        }
    }

    public override void AbortBehaviors()
    {
        _chasePlayer?.Disable();
        _enemyAttack?.Disable();
        _stunned?.Disable();
    }

    public bool CloseEnough()
    {
        return (_player.transform.position - transform.position).magnitude < _chasePlayer.AttackRange;
    }
}
