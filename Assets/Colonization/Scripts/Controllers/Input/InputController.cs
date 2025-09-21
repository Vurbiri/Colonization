using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;
using static UnityEngine.InputSystem.InputAction;


namespace Vurbiri.Colonization.Controllers
{
    [Serializable]
    public class InputController : /*IReactive<bool>,*/ IDisposable
    {
        private readonly Camera _camera;
        private readonly int _layerMaskRight;
        private readonly float _distance;
        private readonly InputControlAction _inputActions;
        private readonly RBool _windowMode = new(false);

        private ISelectable _selectObj;
        private InputControlAction.GameplayActions _gameplayMap;
        private InputControlAction.CameraActions _cameraMap;
        private InputControlAction.UIActions _UIMap;
        private bool _spectatorMode;

        public InputControlAction.CameraActions CameraActions => _cameraMap;

        public RBool IsWindowMode => _windowMode;

        public InputController(GameEvents events, Camera camera, Settings settings)
        {
            _camera = camera;
            _layerMaskRight = settings.layerMaskRight;
            _distance = settings.distance;

            _inputActions = new();

            _gameplayMap = _inputActions.Gameplay;
            _cameraMap = _inputActions.Camera;
            _UIMap = _inputActions.UI;

            SpectatorMode(true);

            _inputActions.Gameplay.RightClick.performed += OnClickRight;

            events.Subscribe(GameModeId.Play, SpectatorMode);
            events.Subscribe(GameModeId.Landing, SpectatorMode);
            events.Subscribe(GameModeId.EndTurn, SpectatorModeOn);
        }

        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Select(ISelectable selectObj)
        {
            _selectObj?.Unselect(selectObj);
            _selectObj = selectObj;
            _selectObj.Select();
        }

        public void Unselect()
        {
            _selectObj?.Unselect(null);
            _selectObj = null;
        }

        public void Dispose() => _inputActions.Dispose();

        private void SpectatorModeOn(TurnQueue turnQueue, int hexId) => SpectatorMode(true);
        private void SpectatorMode(TurnQueue turnQueue, int hexId) => SpectatorMode(turnQueue.IsNotPerson);
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

        private void OnClickRight(CallbackContext ctx) 
        {
            Ray ray = _camera.ScreenPointToRay(ctx.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out RaycastHit hit, _distance, _layerMaskRight) && TryGetSelectable(hit.collider, out ISelectable selectObj))
                Select(selectObj);

            #region Local: TryGetSelectable(..)
            //=================================
            static bool TryGetSelectable(Collider collider, out ISelectable selectObj)
            {
                if (!collider.TryGetComponent(out selectObj) && collider.TryGetComponent(out ISelectableReference selectRef))
                    selectObj = selectRef.Selectable;

                return selectObj != null;
            }
            #endregion
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings
        {
            public LayerMask layerMaskRight;
            public float distance = 900f;
        }
        #endregion
    }
}

