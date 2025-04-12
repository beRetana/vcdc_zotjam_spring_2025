using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;
    private PlayerController _playerController;
    private PlayerAttacks _playerAttacks;
    private Vector2 _movementInput;

    void Start()
    {
        _playerController = new();
        _playerAttacks = GetComponent<PlayerAttacks>();
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
        SwitchFacing();
        _playerAttacks.FacingRight = (int)_movementInput.x;
    }

    void SwitchFacing()
    {
        transform.localScale = transform.localScale*-1;
    }

    void Dash(InputAction.CallbackContext context)
    {

    }

    void Update()
    {
        transform.position += new Vector3(_movementInput.x, 0f ,_movementInput.y) * _movementSpeed * Time.deltaTime;
    }
}
