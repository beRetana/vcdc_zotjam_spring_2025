using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    private PlayerController _playerController;

    void Start()
    {
        _playerController = new();
        Enable();
    }

    void Enable()
    {
        _playerController.PlayerActions.FirstSelection.performed += FirstAction;
        _playerController.PlayerActions.SecondSelection.performed += SecondAction;
        _playerController.PlayerActions.ThirdSelection.performed += ThirdAction;
        _playerController.PlayerActions.Enable();
    }


    void OnDisable()
    {
        _playerController.PlayerActions.FirstSelection.performed -= FirstAction;
        _playerController.PlayerActions.SecondSelection.performed -= SecondAction;
        _playerController.PlayerActions.ThirdSelection.performed -= ThirdAction;
    }

    public void FirstAction(InputAction.CallbackContext context) => TakeAction(1);
    public void SecondAction(InputAction.CallbackContext context) => TakeAction(2);
    public void ThirdAction(InputAction.CallbackContext context) => TakeAction(3);

    private void TakeAction(int actionNum)
    {
        Debug.Log($"Taking Action: {actionNum}");
    }

}
