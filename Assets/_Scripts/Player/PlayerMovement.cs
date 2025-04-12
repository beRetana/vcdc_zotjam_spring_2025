using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _movementSpeed = 10;
    [SerializeField, Range(0f, 100f)] private float _dashTime = 2f;
    [SerializeField, Range(0f, 100f)] private float _dashSpeed = 5f;
    private PlayerController _playerController;
    private PlayerAttacks _playerAttacks;
    private Rigidbody _rigidbody;
    private Vector2 _movementInput;

    void Start()
    {
        _playerController = new();
        _playerAttacks = GetComponent<PlayerAttacks>();
        _rigidbody = GetComponent<Rigidbody>();
        SetUpMovement();
    }

    void SetUpMovement()
    {
        _playerController.PlayerActions.WASD.performed += HandleMovementInput;
        _playerController.PlayerActions.WASD.started += HandleMovementInput;
        _playerController.PlayerActions.WASD.canceled += HandleMovementInput;
        _playerController.PlayerActions.Dash.performed += Dash;
        EnableMovement();
    }

    void OnDisable()
    {
        _playerController.PlayerActions.WASD.performed -= HandleMovementInput;
        _playerController.PlayerActions.WASD.started -= HandleMovementInput;
        _playerController.PlayerActions.WASD.canceled -= HandleMovementInput;
        _playerController.PlayerActions.Dash.performed -= Dash;
        DisableMovement();
    }

    public void DisableMovement()
    {
        _playerController.PlayerActions.Disable();
    }

    public void EnableMovement()
    {
        _playerController.PlayerActions.Enable();
    }

    void HandleMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.action.ReadValue<Vector2>();
        SwitchFacing((int)_movementInput.x);
        _playerAttacks.FacingRight = (int)transform.localScale.x;
    }

    void SwitchFacing(int side)
    {
        switch (side)
        {
            case 1: // 1 means facing right
                if (transform.localScale.x < 0) 
                    transform.localScale *= -1;
                break;
            case -1: // -1 means facing left
                if (transform.localScale.x > 0)
                    transform.localScale *= -1;
                break;
        }
    }

    void Dash(InputAction.CallbackContext context)
    {
        StartCoroutine(Dashing());
    }

    IEnumerator Dashing()
    {
        _rigidbody.AddForce(new Vector3(_movementInput.x, 0f, _movementInput.y) * _dashSpeed, ForceMode.Impulse);
        DisableMovement();
        yield return new WaitForSeconds(_dashTime);
        EnableMovement();
        _rigidbody.linearVelocity = Vector3.zero;
    }

    private void Update()
    {
        _rigidbody.position += new Vector3(_movementInput.x, 0f, _movementInput.y) * _movementSpeed * Time.deltaTime;
    }
}
