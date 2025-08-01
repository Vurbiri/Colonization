using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


namespace Vurbiri.Colonization.Controllers
{
    [Serializable]
    public class InputController : IDisposable
    {
        private readonly Camera _camera;
        private readonly int _layerMaskRight;
        private readonly float _distance;
        private readonly InputControlAction _inputActions;

        private ISelectable _selectObj;
        private InputControlAction.GameplayActions _gameplayMap;
        private InputControlAction.CameraActions _cameraMap;
        private InputControlAction.UIActions _UIMap;

        public InputControlAction.CameraActions CameraActions => _cameraMap;

        public InputController(GameEvents events, Camera camera, Settings settings)
        {
            _camera = camera;
            _layerMaskRight = settings.layerMaskRight;
            _distance = settings.distance;

            _inputActions = new();

            _gameplayMap = _inputActions.Gameplay;
            _cameraMap = _inputActions.Camera;
            _UIMap = _inputActions.UI;

            _inputActions.Gameplay.RightClick.performed += OnClickRight;

            events.Subscribe(GameModeId.Play, (turn, _) => UIMode(turn.IsNotPerson));
            events.Subscribe(GameModeId.Landing, (turn, _) => UIMode(turn.IsNotPerson));
            events.Subscribe(GameModeId.WaitRoll, (turn, _) => UIMode(turn.IsNotPerson));

            events.Subscribe(GameModeId.EndTurn, (_, _) => { UIMode(true); });
        }

        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();

        public void UIMode(bool enable)
        {
            if (enable)
            {
                _gameplayMap.Disable(); _cameraMap.Disable();
            }
            else
            {
                _gameplayMap.Enable(); _cameraMap.Enable();
            }
        }

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

