using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class CrossroadRoadsMenu : AWorldMenu
    {
        [SerializeField] private WorldHintButton _buttonBack;
        [Space]
        [SerializeField] private IdSet<LinkId, ButtonBuildRoad> _roadButtons;

        private RectTransform _thisTransform;
        private Vector2 _localPoint;
        private Camera _camera;
        private CrossroadMainMenu _mainMen;
        private Crossroad _currentCrossroad;

        public ISubscription<IMenu, bool> Init(CrossroadMainMenu mainMenu)
        {
            _mainMen = mainMenu;
            _thisTransform = GetComponent<RectTransform>();

            _buttonBack.Init(OnClose);

            var roadCost = GameContainer.Prices.Road;
            foreach (var button in _roadButtons)
                button.Init(roadCost, this);

            var cameraTransform = GameContainer.CameraTransform;
            _camera = cameraTransform.Camera;
            cameraTransform.Subscribe(LookAtCamera, false);

            CloseInstant();
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

            base.Open();
        }

        private void OnClose()
        {
            base.Close();
            _mainMen.Open();
        }

        protected override void Disable()
        {
            base.Disable();
            _currentCrossroad = null;
        }

        private void LookAtCamera(Transform cameraTransform)
        {
            if (_currentCrossroad != null)
            {
                foreach (var link in _currentCrossroad.Links)
                {
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                        _roadButtons[link.Id].LocalPosition = _localPoint;
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_roadButtons.Fullness < _roadButtons.Count)
                _roadButtons.ReplaceRange(GetComponentsInChildren<ButtonBuildRoad>());
        }
#endif
    }
}
