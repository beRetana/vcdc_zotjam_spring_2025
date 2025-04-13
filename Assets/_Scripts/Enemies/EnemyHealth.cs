using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField, Range(1f, 500f)] protected float _maxHealth;

    protected float _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    protected virtual void ModifyHealth(float modifier)
    {
        _health = Mathf.Min(_health - modifier, 0f);
        if (_health <= 0f) OnDeath();
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
