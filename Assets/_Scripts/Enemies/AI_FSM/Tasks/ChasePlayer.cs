using UnityEngine;
using AI_FSM;

public class ChasePlayer : TaskBase
{
    [SerializeField, Range(0f, 10f)] private float _attackRange = 5f;

    private bool _startChasing;
    private GameObject _player;
    private Enemy _actor;

    private void Start()
    {
        _actor = GetComponent<Enemy>();
        _player = PlayerMovement.Instance.gameObject;
        _aiController = GetComponent<AIController>();
        _aiController.SetStoppingDistance(_attackRange);
    }
    public override void Disable()
    {
        base.Disable();
        _startChasing = false;
    }

    public override void Enable()
    {
        base.Enable();
        _startChasing = true;
    }

    private void Update()
    {
        if (!_startChasing) return;
        _aiController.MoveTo(_player.transform.position);
        if (CloseEnough())
        {
            _actor.UpdateBehaviour(Enemy.EnemyState.Attacking);
            Disable();
        }
    }

    private bool CloseEnough()
    {
        return ((_player.transform.position - transform.position).magnitude < _attackRange);
    }
}
