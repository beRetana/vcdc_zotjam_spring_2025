using AI_FSM;
using UnityEngine;

public class Enemy : AIActor
{
    protected ChasePlayer _chasePlayer;
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
        _chasePlayer = GetComponent<ChasePlayer>();
        _chasePlayer.Enable();
        _player = PlayerMovement.Instance.gameObject;
        _enemyState = EnemyState.Chasing;
        _controller = GetComponent<AIController>();
        _controller.SetRotationActive(false);
    }

    public void UpdateBehaviour(EnemyState newEnemyState)
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
                break;
            case EnemyState.Stunned:
                break;
        }
    }

    public override void AbortBehaviors()
    {
        _chasePlayer.Disable();
        // attack
        // stunned
    }
}
