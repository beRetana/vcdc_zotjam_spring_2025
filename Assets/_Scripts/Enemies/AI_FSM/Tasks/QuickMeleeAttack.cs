using UnityEngine;

public class QuickMeleeAttack : EnemyAttack
{
    public override void Enable()
    {
        base.Enable();
    }
    public override void Attack()
    {
        float direction = GetPlayerDirection().x;
        direction /= Mathf.Abs(direction);
        RaycastHit hit;
        if (!Physics.SphereCast(transform.position, _attackArea, Vector3.right * direction, out hit, _range)) return;
        
    }

    private Vector3 GetPlayerDirection()
    {
        return (PlayerMovement.Instance.transform.position - transform.position).normalized;
    }
}
