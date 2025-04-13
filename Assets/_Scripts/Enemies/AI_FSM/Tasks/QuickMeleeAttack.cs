using UnityEngine;
using System.Collections;
using static UnityEngine.UI.Image;

public class QuickMeleeAttack : EnemyAttack
{
    [SerializeField] protected float _timeBetweenAttacks = .1f;

    protected Enemy _enemy;

    protected override void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    public override void Enable()
    {
        Attack();
    }

    public override void Disable()
    {
        StopCoroutine(AttackTimer());
    }

    public override void Attack()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.ShankAttack,this.transform.position);

        RaycastHit hit;
        Vector3 direction = GetPlayerDirection();
        if (Physics.Raycast(transform.position  + Vector3.up, direction, out hit, _range))
        {
            Damage(hit.transform.GetComponent<Health>(), _damage);
        }
        StartCoroutine(AttackTimer());
    }

    private void Damage(Health health, int damage)
    {
        while (damage > 0)
        {
            health?.LoseHealth();
            --damage;
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSecondsRealtime(_timeBetweenAttacks);
        if (_enemy.CloseEnough()) Attack();
        else _enemy.UpdateBehaviour(Enemy.EnemyState.Chasing);
    }

    private Vector3 GetPlayerDirection()
    {
        return (PlayerMovement.Instance.transform.position - transform.position).normalized;
    }
}
