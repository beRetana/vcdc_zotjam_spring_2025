using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private const string SPRINTING = "Sprinting";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Sprinting()
    {

    }
}
