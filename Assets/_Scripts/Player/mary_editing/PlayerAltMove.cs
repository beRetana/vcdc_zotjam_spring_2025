using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAltMove : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _baseSpeed = 5;
    [SerializeField, Range(0f, 100f)] private float _sprintSpeed = 7;
    [SerializeField, Range(0f, 100f)] private float _dashTime = .5f;
    [SerializeField, Range(0f, 100f)] private float _dashSpeed = 10f;

    //mary funny
    [SerializeField] private Love happyScript;

    private PlayerController _playerController;
    private PlayerAlternateAttacks _playerAttacks;
    private Rigidbody _rigidbody;
    private Vector2 _movementInput;

    private float _movementSpeed;

    void Start()
    {
        _playerController = new();
        _playerAttacks = GetComponent<PlayerAlternateAttacks>();
        _rigidbody = GetComponent<Rigidbody>();
        _movementSpeed = _baseSpeed;
        SetUpMovement();
    }

    void SetUpMovement()
    {
        _playerController.PlayerActions.WASD.performed += HandleMovementInput;
        _playerController.PlayerActions.WASD.started += HandleMovementInput;
        _playerController.PlayerActions.WASD.canceled += HandleMovementInput;
        _playerController.PlayerActions.Dash.performed += Dash;
        _playerController.PlayerActions.Sprint.started += StartSprint;
        _playerController.PlayerActions.Sprint.canceled += EndSprint;
        // EnableMovement();
    }

    void OnDisable()
    {
        _playerController.PlayerActions.WASD.performed -= HandleMovementInput;
        _playerController.PlayerActions.WASD.started -= HandleMovementInput;
        _playerController.PlayerActions.WASD.canceled -= HandleMovementInput;
        _playerController.PlayerActions.Dash.performed -= Dash;
        // DisableMovement();
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

    void StartSprint(InputAction.CallbackContext context)
    {
        _movementSpeed = _sprintSpeed;
    }

    void EndSprint(InputAction.CallbackContext context)
    {
        _movementSpeed = _baseSpeed;
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
        transform.position += new Vector3(_movementInput.x, 0f, _movementInput.y) * _movementSpeed * Time.deltaTime;
    }

    // mary test
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Collided with: " + collision.gameObject.name);

        happyScript.editHerLove(2);
    }
}
