using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField, Range(1f, 500f)] protected float _maxHealth;
    [SerializeField, Range(1f, 100f)] protected int _rewardPoints;

    protected float _health;

    public Action OnEnemyAttacked;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void SetOnEnemyAttacked(Action action)
    {
        OnEnemyAttacked += action;
    }

    public virtual void ModifyHealth(float modifier)
    {
        _health = Mathf.Max(_health - modifier, 0f);
        if (_health <= 0f) OnDeath();
        else OnEnemyAttacked?.Invoke();
    }

    protected virtual void OnDeath()
    {
        PlayerMovement.Instance.GetComponent<Points>().EnemyDeath(_rewardPoints);
    }
}
