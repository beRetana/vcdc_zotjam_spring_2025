using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private const string SPRINTING = "Sprinting";
    private const string DASHING = "Dashing";
    private const string DASHED = "Dashed";
    private const string WALKING = "Walking";

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
        _animator.SetTrigger(DASHED);
    }

    public void Walking(bool active)
    {
        _animator.SetBool(WALKING, active);
    }


}
