using UnityEngine;
using UnityEngine.UI;

public class CrossroadRoadsMenu : ACrossroadBuildMenu
{
    [SerializeField] private CmButton[] _roadButtons;

    private RectTransform _thisTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private Vector2 _localPoint;

    private readonly RectTransform[] _buttonsTransform = new RectTransform[COUNT_ROADS];
    private readonly Graphic[] _buttonsGraphic = new Graphic[COUNT_ROADS];

    private const int COUNT_ROADS = 3;

    public override void Initialize(ACrossroadMenu mainMenu)
    {
        base.Initialize(mainMenu);
        
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _cameraTransform = Camera.main.transform;

        CmButton button;
        for (int i = 0; i < COUNT_ROADS; i++)
        {
            button = _roadButtons[i];
            _buttonsTransform[i] = button.GetComponent<RectTransform>();
            _buttonsGraphic[i] = button.targetGraphic;
        }

        gameObject.SetActive(false);
    }

    public override void Open(Crossroad crossroad)
    {
        _currentCrossroad = crossroad;
        Color currentColor = _players.Current.Color;

        CmButton button; int i = 0;
        foreach (var link in crossroad.Links)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                _buttonsTransform[i].anchoredPosition = _localPoint;

            button = _roadButtons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClick(link));
            button.interactable = link.Owner == PlayerType.None;
            _buttonsGraphic[i].color = button.interactable ? currentColor : _players[link.Owner].Color;
            
            button.gameObject.SetActive(true);

            i++;
        }

        for (; i < COUNT_ROADS; i++)
        {
            button = _roadButtons[i];
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }

        _lastCameraPosition = _cameraTransform.position;
        gameObject.SetActive(true);

        #region Local: OnClick()
        //=================================
        void OnClick(CrossroadLink link)
        {
            link.SetStart(_currentCrossroad);
            _players.Current.BuildRoad(link);

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
}
