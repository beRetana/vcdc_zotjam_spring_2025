using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;
    private PlayerController _playerController;
    private Vector2 _movementInput;
    private float _movement;

    void Start()
    {
        _playerController = new();
        Enable();
    }

    void Enable()
    {
        _playerController.PlayerActions.WASD.performed += HandleMovementInput;
        _playerController.PlayerActions.WASD.started += HandleMovementInput;
        _playerController.PlayerActions.WASD.canceled += HandleMovementInput;
        _playerController.PlayerActions.Dash.performed += Dash;
        _playerController.PlayerActions.Enable();
    }

    void OnDisable()
    {
        _playerController.PlayerActions.WASD.performed -= HandleMovementInput;
        _playerController.PlayerActions.WASD.started -= HandleMovementInput;
        _playerController.PlayerActions.WASD.canceled -= HandleMovementInput;
        _playerController.PlayerActions.Dash.performed -= Dash;
        _playerController.PlayerActions.Disable();
    }

    public void DisableMovement()
    {
        _playerController.PlayerActions.Disable();
    }

    void HandleMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.action.ReadValue<Vector2>();
        Debug.Log(_movementInput);
    }

    void Dash(InputAction.CallbackContext context)
    {

    }

    void Update()
    {
        transform.position += new Vector3(_movementInput.x, _movementInput.y, 0f) * _movementSpeed * Time.deltaTime;
    }
}
