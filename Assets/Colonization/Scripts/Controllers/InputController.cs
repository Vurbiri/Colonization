using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


namespace Vurbiri.Colonization.Controllers
{
    public class InputController : IDisposable
    {
        private readonly Camera _camera;
        private int _layerMask;
        private readonly float _distance = 900f;
        private readonly InputControlAction _inputActions;

        private ISelectable _selectObj;

        public InputControlAction.CameraActions CameraActions => _inputActions.Camera;

        public InputController(Camera camera, Settings settings)
        {
            _camera = camera;
            _layerMask = settings.layerMask;
            _distance = settings.distance;

            _inputActions = new();
            _inputActions.Enable();

            DisableGameplayMap();

            //_inputActions.Gameplay.LeftClick.performed += OnLeftClick;
            _inputActions.Gameplay.RightClick.performed += OnRightClick;
        }

        public void DisableGameplayMap()
        {
            _inputActions.Gameplay.Disable();
            _inputActions.Camera.Disable();
        }

        public void EnableGameplayMap()
        {
            _inputActions.Gameplay.Enable();
            _inputActions.Camera.Enable();
        }

        private void OnRightClick(CallbackContext ctx)
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

