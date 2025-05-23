//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\CrossroadRoadsMenu.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CrossroadRoadsMenu : AWorldMenu
    {
        [Space]
        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private WorldHintButton _buttonBack;
        [Space]
        [SerializeField] private IdSet<LinkId, ButtonBuildRoad> _roadButtons;

        private RectTransform _thisTransform;
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private Vector2 _localPoint;
        private CrossroadMainMenu _mainMen;
        private Crossroad _currentCrossroad;

        public ISubscription<IMenu, bool> Init(CrossroadMainMenu mainMenu, ContextMenuSettings settings)
        {
            ACurrencies roadCost = settings.prices.Road;
            _mainMen = mainMenu;
            _camera = settings.camera;

            _buttonBack.Init(settings.hint, OnClose);

            _thisTransform = GetComponent<RectTransform>();
            _cameraTransform = _camera.transform;

            foreach (var button in _roadButtons)
                button.Init(settings, roadCost, this);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;

            CrossroadLink link;
            foreach (var button in _roadButtons)
            {
                if (button.Setup(crossroad, out link))
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                        button.LocalPosition = _localPoint;
            }

            _lastCameraPosition = _cameraTransform.position;

            base.Open();
        }

        private void OnClose()
        {
            base.Close();
            _mainMen.Open();
        }

        private void Update()
        {
            if (_currentCrossroad == null || _lastCameraPosition == _cameraTransform.position)
                return;

            foreach (var link in _currentCrossroad.Links)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                    _roadButtons[link.Id].LocalPosition = _localPoint;
            }
            _lastCameraPosition = _cameraTransform.position;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_roadButtons.Filling < _roadButtons.Count)
                _roadButtons.ReplaceRange(GetComponentsInChildren<ButtonBuildRoad>());
        }
#endif
    }
}
