using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField, Range(1f, 500f)] protected float _maxHealth;

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
        Debug.Log($"The Enemy Health is: {_health}");
        if (_health <= 0f) OnDeath();
        else OnEnemyAttacked?.Invoke();
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
