using System;
using UnityEngine;
using Vurbiri.Reactive;
using static UnityEngine.InputSystem.InputAction;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Controllers
{
    [Serializable]
    public class InputController : IDisposable
    {
        private readonly Camera _camera;
        private readonly float _distance;
        private readonly InputControlAction _inputActions;
        private readonly RBool _windowMode = new(false);

        private ISelectable _selectObj;
        private InputControlAction.GameplayActions _gameplayMap;
        private InputControlAction.CameraActions _cameraMap;
        private InputControlAction.UIActions _UIMap;
        private bool _spectatorMode;

        public InputControlAction.CameraActions CameraActions { [Impl(256)] get => _cameraMap; }
        public RBool IsWindowMode { [Impl(256)] get => _windowMode; }

        public InputController(GameEvents events, Camera camera, Settings settings)
        {
            _camera = camera;
            _distance = settings.distance;

            _inputActions = new();

            _gameplayMap = _inputActions.Gameplay;
            _cameraMap = _inputActions.Camera;
            _UIMap = _inputActions.UI;

            SpectatorMode(true);

            _inputActions.Gameplay.RightClick.performed += OnRightClick;
            _inputActions.Gameplay.LeftClick.performed += OnLeftClick;

            events.Subscribe(GameModeId.Play, SpectatorMode);
            events.Subscribe(GameModeId.Landing, SpectatorMode);
            events.Subscribe(GameModeId.EndTurn, SpectatorModeOn);
        }

        [Impl(256)] public void Enable() => _inputActions.Enable();
        [Impl(256)] public void Disable() => _inputActions.Disable();

        public void WindowMode(bool enable)
        {
            if (_windowMode != enable)
            {
                if (_windowMode.Value = enable)
                {
                    _gameplayMap.Disable(); _cameraMap.Disable(); _UIMap.Enable();
                }
                else
                {
                    if(!_spectatorMode) 
                        _gameplayMap.Enable(); 
                    _cameraMap.Enable();
                }
            }
        }

        [Impl(256)] public void Select(ISelectable selectObj, MouseButton button)
        {
            _selectObj?.Unselect(selectObj);
            _selectObj = selectObj;
            _selectObj.Select(button);
        }

        [Impl(256)] public void Unselect()
        {
            _selectObj?.Unselect(null);
            _selectObj = null;
        }

        public void Dispose() => _inputActions.Dispose();

        private void SpectatorModeOn(TurnQueue turnQueue, int hexId) => SpectatorMode(true);
        private void SpectatorMode(TurnQueue turnQueue, int hexId) => SpectatorMode(!turnQueue.isPerson);
        private void SpectatorMode(bool enable)
        {
            if (_spectatorMode != enable)
            {
                if (_spectatorMode = enable)
                {
                    _gameplayMap.Disable(); _cameraMap.Enable(); _UIMap.Enable();
                }
                else
                {
                    _gameplayMap.Enable();
                }
            }
        }

        private void OnRightClick(CallbackContext ctx) => OnClick(ctx.ReadValue<Vector2>(), LayersMask.SelectableRight, MouseButton.Right);
        private void OnLeftClick(CallbackContext ctx) => OnClick(ctx.ReadValue<Vector2>(), LayersMask.SelectableLeft, MouseButton.Left);

        private void OnClick(Vector3 point, int mask, MouseButton button)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(point), out RaycastHit hit, _distance, mask) && TryGetSelectable(hit.collider, out ISelectable selectObj))
                Select(selectObj, button);

            //=========== Local ================
            static bool TryGetSelectable(Collider collider, out ISelectable selectObj)
            {
                if (!collider.TryGetComponent(out selectObj) && collider.TryGetComponent(out ISelectableReference selectRef))
                    selectObj = selectRef.Selectable;

                return selectObj != null;
            }
        }


        //************* Nested ****************
        [Serializable] public class Settings
        {
            public float distance = 900f;
        }
    }
}

