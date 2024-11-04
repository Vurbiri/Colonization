using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadRoadsMenu : ACrossroadMenu
    {
        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private HintingButton _buttonBack;
        [Space]
        [SerializeField] private IdHashSet<LinkId, ButtonBuildRoad> _roadButtons;

        private RectTransform _thisTransform;
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private Vector2 _localPoint;
        private CrossroadMainMenu _mainMen;

        public void Init(CrossroadMainMenu mainMenu, Players players, Color color ,Camera camera, ACurrencies roadCost)
        {
            _mainMen = mainMenu;
            _camera = camera;

            _buttonBack.Init(OnClose);

            _thisTransform = GetComponent<RectTransform>();
            _cameraTransform = _camera.transform;

            foreach (var button in _roadButtons)
                button.Init(players, color, roadCost, _thisGO);

            _thisGO.SetActive(false);
        }

        public override void Open(Crossroad crossroad)
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

            _thisGO.SetActive(true);
        }

        protected override void OnClose()
        {
            _thisGO.SetActive(false);
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
    }
}
