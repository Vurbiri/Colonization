using UnityEngine;
using UnityEngine.UI;

public class TestUi : MonoBehaviour
{
    [SerializeField] private Crossroads _crossroads;
    [SerializeField] private Roads _roads;
    [Space]
    [SerializeField] private RectTransform _imageTransform;
    [SerializeField] private Button[] _roadButtons;

    private Players _players;
    private RectTransform _thisTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    private Vector2 _localPoint;
    private Vector3 _lastCameraPosition;
    private Crossroad _crossCurrent;
    private int _countCurrent;
    private bool _built; //test

    private RectTransform[] _roadButtonsTransform = new RectTransform[COUNT_ROADS];
    private Graphic[] _roadButtonsGraphic = new Graphic[COUNT_ROADS];

    private readonly Quaternion _angleX180 = Quaternion.Euler(180f, 0f, 0f);
    private const int COUNT_ROADS = 3;

    private void Awake()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _cameraTransform = _camera.transform;

        _crossroads.EventSelectCrossroad += OnSelectCrossroad;

        Button button;
        _imageTransform.gameObject.SetActive(false);
        for (int i = 0; i < COUNT_ROADS; i++)
        {
            button = _roadButtons[i];
            _roadButtonsTransform[i] = button.GetComponent<RectTransform>();
            _roadButtonsGraphic[i] = button.targetGraphic;
            button.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _players = Players.Instance;
    }

    private void OnSelectCrossroad(Crossroad cross)
    {
        int i = 0;
        if (cross.CanRoadsBuilt(_players.Current.Type) || !_built)
        {
            _crossCurrent = cross;
            _countCurrent = cross.Links.Count;

            SetPositionUI();
            LookAtCamera();

            Button button;
            foreach (var link in _crossCurrent.Links)
            {
                button = _roadButtons[i];
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnClick(link));
                button.interactable = link.Owner == PlayerType.None;

                _roadButtonsGraphic[i].color = _players.Current.Color;
                _roadButtonsTransform[i].gameObject.SetActive(true);

                i++;
            }
            _imageTransform.gameObject.SetActive(true);
        }

        for(; i < COUNT_ROADS; i++)
        {
            _roadButtons[i].onClick.RemoveAllListeners();
            _roadButtonsTransform[i].gameObject.SetActive(false);
        }

        #region Local: OnClick()
        //=================================
        void OnClick(CrossroadLink road)
        {
            _built = true;

            road.SetStart(_crossCurrent);
            _roads.BuildRoad(road, _players.Current);

            _imageTransform.gameObject.SetActive(false);
            for (int i = 0; i < _countCurrent; i++)
                _roadButtonsTransform[i].gameObject.SetActive(false);

            _crossCurrent = null;
            _countCurrent = 0;
        }
        #endregion
    }

    private void LookAtCamera()
    {
        _imageTransform.LookAt(_cameraTransform);
        _imageTransform.localRotation *= _angleX180;
        for (int i = 0; i < _countCurrent; i++)
        {
            _roadButtonsTransform[i].LookAt(_cameraTransform);
            _roadButtonsTransform[i].localRotation *= _angleX180;
        }

        _lastCameraPosition = _cameraTransform.position;
    }

    private void SetPositionUI()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(_crossCurrent.Position), _camera, out _localPoint))
            _imageTransform.anchoredPosition = _localPoint;

        int i = 0;
        foreach (var link in _crossCurrent.Links)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(link.Position), _camera, out _localPoint))
                _roadButtonsTransform[i].anchoredPosition = _localPoint;

            i++;
        }
    }

    private void Update()
    {
        if (_crossCurrent == null || _lastCameraPosition == _cameraTransform.position) return;

        LookAtCamera();
    }
}
