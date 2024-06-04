using System.Collections.Generic;
using UnityEngine;


public class TestUi : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private RectTransform _imageTransform;
    [SerializeField] private RectTransform[] _roads;

    private RectTransform _thisTransform;
    private Map _map;
    private Camera _camera;
    private Vector2 _localPoint;
    private Crossroad _crossCurrent;
    private List<RectTransform> _roadsCurrent;

    private Vector2 _size;
    private Vector2[] _sizes = new Vector2[3];

    private void Awake()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;

        _cameraController.EventStartChangeCamera += OffUI;
        _cameraController.EventEndChangeCamera += SetUI;

        _map = _cameraController.Map;
        _map.EventSelectCrossroad += OnSelectCrossroad;

        _roadsCurrent = new (_roads.Length);

        _imageTransform.gameObject.SetActive(false);

        for (int i = 0; i < _roads.Length; i++)
        {
            _roads[i].gameObject.SetActive(false);
            _sizes[i] = _roads[i].sizeDelta;
        }

        _size = _imageTransform.sizeDelta;
    }

    private void OnSelectCrossroad(Crossroad cross)
    {
        _crossCurrent = cross;


        //for(int i = cross.Roads.Count - 1; i < _roads.Length; i++)
        //    _roads[i].gameObject.SetActive(false);

        //SetUI();
    }

    private void OffUI()
    {
        _imageTransform.gameObject.SetActive(false);

        for (int i = 0; i < _roads.Length; i++)
            _roads[i].gameObject.SetActive(false);
    }

    private void SetUI()
    {
        if (_crossCurrent == null)
            return;

        float ratioSize = _camera.transform.position.y / 250f;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(_crossCurrent.Position), _camera, out _localPoint))
        {
            _imageTransform.anchoredPosition = _localPoint;
            _imageTransform.sizeDelta = _size / ratioSize;
            _imageTransform.gameObject.SetActive(true);
            
        }

        int i = 0;
        foreach (var road in _crossCurrent.Roads)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(road.Position), _camera, out _localPoint))
            {
                _roads[i].anchoredPosition = _localPoint;
                _roads[i].sizeDelta = _sizes[i] / ratioSize;
                _roads[i].gameObject.SetActive(true);
            }

            i++;
        }
    }
}
