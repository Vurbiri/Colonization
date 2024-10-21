using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadRoadsMenu : ACrossroadMenu
    {
        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private CmButton _buttonBack;
        [Space]
        [SerializeField] private IdHashSet<LinkId, ButtonBuildRoad> _roadButtons;

        private RectTransform _thisTransform;
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private Vector2 _localPoint;
        private CrossroadMainMenu _mainMen;

        public void Init(CrossroadMainMenu mainMenu, Players players, Camera camera, ACurrencies roadCost)
        {
            _mainMen = mainMenu;
            _camera = camera;

            _buttonBack.onClick.AddListener(OnBack);

            _thisTransform = GetComponent<RectTransform>();
            _cameraTransform = _camera.transform;

            foreach (var button in _roadButtons)
            {
                button.Init(players, roadCost);
                button.AddListener(OnClick);
            }

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

        private void OnBack()
        {
            _thisGO.SetActive(false);
            _mainMen.Open();
        }

        private void OnClick()
        {
            _thisGO.SetActive(false);
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
