//Assets\Colonization\Scripts\Controllers\InputController.cs
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


namespace Vurbiri.Colonization.Controllers
{
    [Serializable]
    public class InputController : IDisposable
    {
        private readonly Camera _camera;
        private readonly int _layerMask;
        private readonly float _distance = 900f;
        private readonly InputControlAction _inputActions;

        private ISelectable _selectObj;
        private InputControlAction.GameplayActions _gameplayMap;
        private InputControlAction.CameraActions _cameraMap;
        private InputControlAction.UIActions _UIMap;

        public InputControlAction.CameraActions CameraActions => _cameraMap;

        public bool GameplayMap { get => _gameplayMap.enabled;  set { if (value) _gameplayMap.Enable(); else _gameplayMap.Disable(); } }
        public bool CameraMap { get => _cameraMap.enabled; set { if (value) _cameraMap.Enable(); else _cameraMap.Disable(); } }
        public bool UIMap { get => _UIMap.enabled; set { if (value) _UIMap.Enable(); else _UIMap.Disable(); } }

        public InputController(Camera camera, Settings settings)
        {
            _camera = camera;
            _layerMask = settings.layerMask;
            _distance = settings.distance;

            _inputActions = new();
            _inputActions.Enable();

            _gameplayMap = _inputActions.Gameplay;
            _cameraMap = _inputActions.Camera;
            _UIMap = _inputActions.UI;

            //_inputActions.Gameplay.LeftClick.performed += OnLeftClick;
            _inputActions.Gameplay.RightClick.performed += OnRightClick;
        }

        public void EnableAll()
        {
            _gameplayMap.Enable(); _cameraMap.Enable(); _UIMap.Enable();
        }
        public void DisableAll()
        {
            _gameplayMap.Disable(); _cameraMap.Disable(); _UIMap.Disable();
        }

        public void OnRightClick(CallbackContext ctx)
        {
            Ray ray = _camera.ScreenPointToRay(ctx.ReadValue<Vector2>());
            if (Physics.Raycast(ray, out RaycastHit hit, _distance, _layerMask) && hit.collider.TryGetComponent(out ISelectable selectObj))
            {
                _selectObj?.Unselect(selectObj);
                _selectObj = selectObj;
                _selectObj.Select();
            }
        }

        public void Dispose() => _inputActions?.Disable();

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings
        {
            public LayerMask layerMask;
            public float distance = 900f;
        }
        #endregion
    }
}

