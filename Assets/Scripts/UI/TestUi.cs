using UnityEngine;

public class TestUi : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private RectTransform _imageTransform;
    [SerializeField] private RectTransform[] _roads;

    private RectTransform _thisTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    private Vector2 _localPoint;
    private Vector3 _lastCameraPosition;
    private Crossroad _crossCurrent;
    private int _countCurrent;

    private void Awake()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _cameraTransform = _camera.transform;

        _map.EventSelectCrossroad += OnSelectCrossroad;

        _imageTransform.gameObject.SetActive(false);
        foreach (var road in _roads)
            road.gameObject.SetActive(false);
    }

    private void OnSelectCrossroad(Crossroad cross)
    {
        _crossCurrent = cross;
        _countCurrent = cross.Roads.Count;

        SetPositionUI();
        LookAtCamera();

        _imageTransform.gameObject.SetActive(true);

        for (int i = 0; i < _roads.Length; i++)
            _roads[i].gameObject.SetActive(i < _countCurrent);
    }

    private void LookAtCamera()
    {
        _imageTransform.LookAt(_cameraTransform);
        for (int i = 0; i < _roads.Length; i++)
            _roads[i].LookAt(_cameraTransform);
    }

    private void SetPositionUI()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(_crossCurrent.Position), _camera, out _localPoint))
            _imageTransform.anchoredPosition = _localPoint;

        int i = 0;
        foreach (var road in _crossCurrent.Roads)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(road.Position), _camera, out _localPoint))
                _roads[i].anchoredPosition = _localPoint;

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
