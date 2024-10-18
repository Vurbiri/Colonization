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
        private ACurrencies _roadCost;

        private readonly RectTransform[] _buttonsTransform = new RectTransform[COUNT_ROADS];

        private const int COUNT_ROADS = 3;

        public void Init(ACrossroadMenu mainMenu, ACurrencies roadCost)
        {
            _players = SceneObjects.Get<Players>();

            _buttonBack.onClick.AddListener(OnBack);

            _roadCost = roadCost;

            _thisTransform = GetComponent<RectTransform>();
            _cameraTransform = _camera.transform;

            ButtonBuild button;
            for (int i = 0; i < COUNT_ROADS; i++)
            {
                button = _roadButtons[i];
                button.Init();
                _buttonsTransform[i] = button.GetComponent<RectTransform>();
            }

            gameObject.SetActive(false);

            #region Local: OnBack()
            //=================================
            void OnBack()
            {
                gameObject.SetActive(false);
                mainMenu.Open();
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
                button.TargetGraphic.color = isOwner ? currentColor : _players[link.Owner].Color;
                button.SetupHint(currentPlayer.Resources, _roadCost);

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
            gameObject.SetActive(true);

            #region Local: OnClick()
            //=================================
            void OnClick(CrossroadLink link)
            {
                _players.Current.RoadBuy(_currentCrossroad, link);

                _currentCrossroad = null;
                gameObject.SetActive(false);
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_camera == null)
                _camera = Camera.main;
        }
#endif
    }
}
