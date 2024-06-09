using System.Collections.Generic;
using UnityEngine;

public class RoadLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _roadRenderer;
    [Space]
    [SerializeField] private float _widthRoad = 1.1f;
    [SerializeField] private Vector2Int _rangeCount = new(2, 5);
    [SerializeField] private float _offsetY = 0.05f;
    [SerializeField] private Vector2 _rateWave = new(0.6f, 1.2f);
    [SerializeField] private Vector2 _lengthFluctuation = new(0.8f, 1.2f);
    [Space]
    [SerializeField] private Vector2 _textureScaleMin = new(0.75f, 0.5f);
    [SerializeField] private Vector2 _textureScaleMax = new(1.25f, 1f);

    private readonly List<Vector3> _points = new();
    private Vector3 _start, _end;
    private Vector2 _textureScale;
    private float _textureScaleX;


    private void Awake()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void Initialize(Vector3 start, Vector3 end)
    {
        start.y = end.y = _offsetY;

        _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;
        _textureScale = URandom.Vector2(_textureScaleMin, _textureScaleMax);
        _textureScaleX = _textureScale.x;

        var alphas = new GradientAlphaKey[4];
        alphas[0] = new(0.0f, 0.0f);
        alphas[1] = new(1.0f, 0.01f);
        alphas[2] = new(1.0f, 0.99f);
        alphas[3] = new(0.0f, 1.0f);
        var gradient = new Gradient();
        gradient.SetKeys(_roadRenderer.colorGradient.colorKeys, alphas);
        _roadRenderer.colorGradient = gradient;

        _points.Add(start);
        _start = start;

        CreateLine(start, end);
    }

    public bool TryAdd(Vector3 start, Vector3 end)
    {
        start.y = end.y = _offsetY;

        if (_start != start && _start != end && _end != start && _end != end)
            return false;

        if(_start == end)
        {
            ListReverse();
            StartEndReverse();
        }
        else if (_start == start)
        {
            ListReverse();
        }
        else if (_end == end)
        {
            StartEndReverse();
        }

        _textureScale.x += _textureScaleX;
        CreateLine(start, end);

        return true;

        #region Local: ListReverse(), StartEndReverse()
        //=================================
        void ListReverse()
        {
            _points.Reverse();
            _start = _end;
        }
        //=================================
        void StartEndReverse()
        {
            _end = start;
            start = end;
            end = _end;
        }
        #endregion
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        int count = URandom.Range(_rangeCount);
        Vector2 wave = _widthRoad / count * _rateWave;
        Vector3 step = (end - start) / (count + 1), offsetSide = Vector3.Cross(Vector3.up, step.normalized);
        float sign = URandom.IsTrue() ? 1f : -1f, signStep = -1f;

        for (int i = 0; i < count; i++)
        {
            sign *= signStep;
            start += URandom.Range(_lengthFluctuation) * step + URandom.Range(wave) * sign * offsetSide;
            _points.Add(start);
        }
        _points.Add(end);
        _end = end;

        _roadRenderer.textureScale = _textureScale;
        _roadRenderer.positionCount = _points.Count;
        _roadRenderer.SetPositions(_points.ToArray());
    }
}
