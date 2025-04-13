using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class rotates the object based on the mode chosen.
/// </summary>
public class LookAtCamera : MonoBehaviour {

    [SerializeField] private Mode mode;

    private Camera _camera;

    private enum Mode{
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    private void Start()
    {
        SetCamera(Camera.main);
    }

    public void SetCamera(Camera newCamera)
    {
        _camera = newCamera;
    }

    private void LateUpdate(){

        if (_camera == null) return;

        switch (mode){
            case Mode.LookAtInverted: 
                transform.LookAt(_camera.transform);
                break;
            case Mode.LookAt:
                Vector3 cameraDirection = transform.position - _camera.transform.position;
                transform.LookAt(transform.position + cameraDirection);
                break;
            case Mode.CameraForward:
                transform.forward = _camera.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -_camera.transform.forward;
                break;
        }
    }
}
