using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadRoadsMenu : ACrossroadMenu
    {
        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private CmButton _buttonBack;
        [SerializeField] private ButtonBuild[] _roadButtons;

        private RectTransform _thisTransform;
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private Vector2 _localPoint;
        private Id<PlayerId> _none = new(PlayerId.None);
        private CrossroadMainMenu _mainMen;
        private readonly RectTransform[] _buttonsTransform = new RectTransform[COUNT_ROADS];
        
        private const int COUNT_ROADS = 3;

        public void Init(CrossroadMainMenu mainMenu, Players players, Camera camera, ACurrencies roadCost)
        {
            _mainMen = mainMenu;
            _players = players;
            _camera = camera;

            _buttonBack.onClick.AddListener(OnBack);

            _thisTransform = GetComponent<RectTransform>();
            _cameraTransform = _camera.transform;

            ButtonBuild button;
            for (int i = 0; i < COUNT_ROADS; i++)
            {
                button = _roadButtons[i];
                button.Init(roadCost);
                _buttonsTransform[i] = button.GetComponent<RectTransform>();
            }

            _thisGO.SetActive(false);

            #region Local: OnBack()
            //=================================
            void OnBack()
            {
                _thisGO.SetActive(false);
                _mainMen.Open();
            }
            #endregion
        }

        public override void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            Player currentPlayer = _players.Current;
            Color currentColor = currentPlayer.Color;

            bool isOwner;
            ButtonBuild button; int i = 0;
            foreach (var link in crossroad.Links)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                    _buttonsTransform[i].anchoredPosition = _localPoint;

                button = _roadButtons[i];
                button.RemoveAllListeners();
                button.AddListener(() => OnClick(link));
                button.Button.interactable = isOwner = link.Owner == _none;
                button.Color = isOwner ? currentColor : _players[link.Owner].Color;
                button.SetupHint(currentPlayer.Resources);

                button.SetActive(true);
                i++;
            }

            for (; i < COUNT_ROADS; i++)
            {
                button = _roadButtons[i];
                button.RemoveAllListeners();
                button.SetActive(false);
            }

            _lastCameraPosition = _cameraTransform.position;

            _thisGO.SetActive(true);

            #region Local: OnClick()
            //=================================
            void OnClick(CrossroadLink link)
            {
                _players.Current.BuyRoad(_currentCrossroad, link);

                _currentCrossroad = null;
                _thisGO.SetActive(false);
            }
            #endregion
        }

        private void Update()
        {
            if (_currentCrossroad == null || _lastCameraPosition == _cameraTransform.position)
                return;

            int i = 0;
            foreach (var link in _currentCrossroad.Links)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                    _buttonsTransform[i].anchoredPosition = _localPoint;

                i++;
            }
            _lastCameraPosition = _cameraTransform.position;
        }
    }
}
