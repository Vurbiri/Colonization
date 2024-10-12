using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


namespace Vurbiri.Colonization
{
    public class InputController : IDisposable
    {
        private readonly Camera _camera;
        private LayerMask _layerLeft;
        private LayerMask _layerRight;
        private readonly float _distance = 900f;

        public InputControlAction.CameraActions CameraActions => _inputActions.Camera;

        private InputControlAction _inputActions;
        private Ray _ray;
        private RaycastHit _hit;
        private ISelectable _obj;

        public InputController(Camera camera, Settings settings)
        {
            _camera = camera;
            _layerLeft = settings.layerLeft;
            _layerRight = settings.layerRight;
            _distance = settings.distance;

            _inputActions = new();
            _inputActions.Enable();

            DisableGameplayMap();

            _inputActions.Gameplay.LeftClick.performed += OnLeftClick;
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

        private void OnLeftClick(CallbackContext ctx) => Click(ctx.ReadValue<Vector2>(), _layerLeft);
        private void OnRightClick(CallbackContext ctx) => Click(ctx.ReadValue<Vector2>(), _layerRight);

        private void Click(Vector2 mousePosition, LayerMask layer)
        {
            _ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(_ray, out _hit, _distance, layer.value) && _hit.collider.TryGetComponent(out _obj))
                _obj.Select();
        }

        public void Dispose() => _inputActions?.Disable();

        #region Nested: GameplayData
        //***********************************
        [Serializable]
        public class Settings
        {
            public LayerMask layerLeft;
            public LayerMask layerRight;
            [Space]
            public float distance = 900f;
        }
        #endregion
    }
}

