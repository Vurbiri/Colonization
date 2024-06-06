using UnityEngine;
using UnityEngine.UI;

public class TestUi : MonoBehaviour
{
    [SerializeField] private Map _map;
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

    private readonly Quaternion _angleX90 = Quaternion.Euler(-90f, 0f, 0f);
    private const int COUNT_ROADS = 3;

    private void Awake()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _cameraTransform = _camera.transform;

        _map.EventSelectCrossroad += OnSelectCrossroad;

        _imageTransform.gameObject.SetActive(false);

        Button button;
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

        _imageTransform.gameObject.SetActive(true);

        int i = 0;
        foreach (var road in _crossCurrent.Roads)
        {
            _roadTransforms[i].gameObject.SetActive(i < _countCurrent);

             _roadButtons[i].onClick.AddListener(() => OnClick(road));
             _roadButtons[i].interactable = road.Owner == Player.None;

            i++;
        }

        #region Local: Zoom_Coroutine()
        //=================================
        void OnClick(Road road)
        {
           
            road.Build(Player.Human);

            _imageTransform.gameObject.SetActive(false);
            for (int i = 0; i < COUNT_ROADS; i++)
            {
                _roadButtons[i].onClick.RemoveAllListeners();
                _roadTransforms[i].gameObject.SetActive(false);
            }

            _crossCurrent = null;
            _countCurrent = 0;
        }
        #endregion
    }

    private void LookAtCamera()
    {
        _imageTransform.localRotation = _cameraTransform.rotation * _angleX90;
        for (int i = 0; i < _countCurrent; i++)
            _roadTransforms[i].localRotation = _cameraTransform.rotation * _angleX90;
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

        _lastCameraPosition = _cameraTransform.position;
    }

    private void Update()
    {
        if (_crossCurrent == null || _lastCameraPosition == _cameraTransform.position) return;

        LookAtCamera();
    }
}
