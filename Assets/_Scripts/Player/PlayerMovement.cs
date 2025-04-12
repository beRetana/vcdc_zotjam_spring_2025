using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;

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

    }

    void Dash(InputAction.CallbackContext context)
    {

    }
}
