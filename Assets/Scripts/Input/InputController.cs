using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-1)]
public class InputController : ASingleton<InputController>
{

    public InputControlAction.CameraActions CameraActions => _inputActions.Camera;


    public event Action EventStartCameraMove;
    public event Action EventStopCameraMove;

    private InputControlAction _inputActions;
    private InputAction _actionMove;

    private Vector2 _cameraMove;

    protected override void Awake()
    {
        _isNotDestroying = false;
        base.Awake();

        _inputActions = new();
        _actionMove = _inputActions.Camera.Move;
    }

    private void Start()
    {
        _inputActions.Enable();

        _actionMove.started += _ => EventStartCameraMove?.Invoke();
        _actionMove.performed += ctx => _cameraMove = ctx.ReadValue<Vector2>();
        _actionMove.canceled += _ => EventStopCameraMove?.Invoke();
    }

    public void EnableGameplayMap()
    {
        _inputActions.General.Enable();
        _inputActions.Camera.Enable();
    }

    private void OnDisable() => _inputActions.Disable();
}

