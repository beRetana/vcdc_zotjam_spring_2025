using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private const string SPRINTING = "Sprinting";
    private const string DASHING = "Dashing";
    private const string DASHED = "Dashed";
    private const string WALKING = "Walking";
    private const string CHARGING = "Charging";
    private const string JAB_ONE = "Jab_One";
    private const string JAB_TWO = "Jab_Two";
    private const string ULTI = "Ulti";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Sprinting(bool active)
    {
        _animator.SetBool(SPRINTING, active);
    }

    public void Dashing(bool active)
    {
        _animator.SetBool(DASHING, active);
    }

    public void Dashed()
    {
        _animator.ResetTrigger(DASHED);
        _animator.SetTrigger(DASHED);
    }

    public void Walking(bool active)
    {
        _animator.SetBool(WALKING, active);
    }

    public void Charging(bool active)
    {
        _animator.SetBool(CHARGING, active);
    }

    public void Jab_One()
    {
        _animator.ResetTrigger(JAB_ONE);
        _animator.SetTrigger(JAB_ONE);
    }

    public void Jab_Two()
    {
        _animator.ResetTrigger(JAB_TWO);
        _animator.SetTrigger(JAB_TWO);
    }

    public void Ulti()
    {
        _animator.ResetTrigger(ULTI);
        _animator.SetTrigger(ULTI);
    }

}
