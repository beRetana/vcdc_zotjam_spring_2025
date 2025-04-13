using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;
    private const string ATTACK = "Attack";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.ResetTrigger(ATTACK);
        _animator.SetTrigger(ATTACK);
    }
}
