using UnityEngine;
using UnityEngine.UI;

public class BuildingRoadsMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonBack;
    [SerializeField] private CustomButton[] _roadButtons;

    private Players _players;
    private RectTransform _thisTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private Vector2 _localPoint;
    private Crossroad _currentCrossroad;

    private readonly RectTransform[] _roadButtonsTransform = new RectTransform[COUNT_ROADS];
    private readonly Graphic[] _roadButtonsGraphic = new Graphic[COUNT_ROADS];

    private const int COUNT_ROADS = 3;

    public void Initialize(BuildingMainMenu mainMenu)
    {
        _thisTransform = GetComponent<RectTransform>();
        _players = Players.Instance;
        _camera = Camera.main;
        _cameraTransform = Camera.main.transform;

        _buttonBack.onClick.AddListener(OnBack);

        CustomButton button;
        for (int i = 0; i < COUNT_ROADS; i++)
        {
            button = _roadButtons[i];
            _roadButtonsTransform[i] = button.GetComponent<RectTransform>();
            _roadButtonsGraphic[i] = button.targetGraphic;
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

    public void Open(Crossroad cross)
    {
        _currentCrossroad = cross;

        CustomButton button; int i = 0;
        foreach (var link in cross.Links)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                _roadButtonsTransform[i].anchoredPosition = _localPoint;

            button = _roadButtons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClick(link));
            button.interactable = link.Owner == PlayerType.None;
            _roadButtonsGraphic[i].color = button.interactable ? _players.Current.Color : _players[link.Owner].Color;
            
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
            link.SetStart(cross);
            _players.Current.BuildRoad(link);

            gameObject.SetActive(false);
        }
        #endregion
    }

    public void Close() => gameObject.SetActive(false);

    private void Update()
    {
        if (_lastCameraPosition == _cameraTransform.position)
            return;

        int i = 0;
        foreach (var link in _currentCrossroad.Links)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                _roadButtonsTransform[i].anchoredPosition = _localPoint;

            i++;
        }
        _lastCameraPosition = _cameraTransform.position;
    }
}
