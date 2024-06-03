using UnityEngine;


public class TestUi : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private RectTransform _imageTransform;
    [SerializeField] private RectTransform[] _roads;

    private RectTransform _thisTransform;
    private Camera _camera;
    private Vector2 _localPoint;

    private void Awake()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _map.EventSelectCrossroad += OnSelectCrossroad;

        _imageTransform.gameObject.SetActive(false);
        foreach (var r in _roads) 
            r.gameObject.SetActive(false);
    }

    private void OnSelectCrossroad(Crossroad cross)
    {

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(cross.Position), _camera, out _localPoint))
        {
            _imageTransform.anchoredPosition = _localPoint;
            _imageTransform.gameObject.SetActive(true);
        }

        int i = 0;
        foreach(var road in cross.Roads)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _camera.WorldToScreenPoint(road.Position), _camera, out _localPoint))
            {
                _roads[i].anchoredPosition = _localPoint;
                _roads[i].gameObject.SetActive(true);
            }
            i++;
        }

        for(;i< _roads.Length; i++)
            _roads[i].gameObject.SetActive(false);
    }
}
