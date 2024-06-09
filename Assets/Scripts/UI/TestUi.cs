using UnityEngine;
using UnityEngine.UI;

public class TestUi : MonoBehaviour
{
    [SerializeField] private Crossroads _crossroads;
    [SerializeField] private RectTransform _imageTransform;
    [SerializeField] private Button[] _roadButtons;

    private RectTransform _thisTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    private Vector2 _localPoint;
    private Vector3 _lastCameraPosition;
    private Crossroad _crossCurrent;
    private int _countCurrent;

    private RectTransform[] _roadTransforms = new RectTransform[COUNT_ROADS];

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
            _roadTransforms[i] = button.GetComponent<RectTransform>();
            button.gameObject.SetActive(false);
        }
    }

    private void OnSelectCrossroad(Crossroad cross)
    {
        _crossCurrent = cross;
        _countCurrent = cross.Roads.Count;

        SetPositionUI();
        LookAtCamera();

        int i = 0; Button button;
        foreach (var road in _crossCurrent.Roads)
        {
            button = _roadButtons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClick(road));
            button.interactable = road.Owner == PlayerType.None;

            _roadTransforms[i].gameObject.SetActive(true);

            i++;
        }
        _imageTransform.gameObject.SetActive(true);

        for(; i < COUNT_ROADS; i++)
        {
            _roadButtons[i].onClick.RemoveAllListeners();
            _roadTransforms[i].gameObject.SetActive(false);
        }

        #region Local: Zoom_Coroutine()
        //=================================
        void OnClick(Road road)
        {
           
            road.Build(PlayerType.Human);

            _imageTransform.gameObject.SetActive(false);
            for (int i = 0; i < _countCurrent; i++)
                _roadTransforms[i].gameObject.SetActive(false);

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
            _roadTransforms[i].LookAt(_cameraTransform);
            _roadTransforms[i].localRotation *= _angleX180;
        }

        _lastCameraPosition = _cameraTransform.position;
    }

    private void SetPositionUI()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(_crossCurrent.Position), _camera, out _localPoint))
            _imageTransform.anchoredPosition = _localPoint;

        int i = 0;
        foreach (var road in _crossCurrent.Roads)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(road.Position), _camera, out _localPoint))
                _roadTransforms[i].anchoredPosition = _localPoint;

            i++;
        }
    }

    private void Update()
    {
        if (_crossCurrent == null || _lastCameraPosition == _cameraTransform.position) return;

        LookAtCamera();
    }
}
