using System;
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
    [Space]
    [SerializeField] private float _alpha = 0.01f;

    private readonly SimpleLinkedList<Vector3> _points = new();
    private Vector3 _first, _last;
    private Vector2 _textureScale;
    private float _textureScaleX;
    private int _countSegments = 0;

    public void Initialize(Vector3 start, Vector3 end, Color color)
    {
        Initialize(start, color);
        start.y = end.y = _offsetY;
        CreateLine(start, end);
    }

    public void Initialize(Vector3 start, Color color)
    {
        start.y = _offsetY;

        _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;
        _textureScale = URandom.Vector2(_textureScaleMin, _textureScaleMax);
        _textureScaleX = _textureScale.x;

        Gradient gradient = new();
        GradientAlphaKey[] alphas = { new(1.0f, 0.0f),new(1.0f, _alpha), new(1.0f, 1f - _alpha), new(0.0f, 1.0f)};
        GradientColorKey[] colors = { new(color, 0.0f), new(color, 1.0f) };
        gradient.SetKeys(colors, alphas);
        _roadRenderer.colorGradient = gradient;

        _points.Add(start);
        _last = _first = start;
    }

    public bool TryAdd(Vector3 start, Vector3 end, bool notAlphaEnd)
    {
        start.y = end.y = _offsetY;

        if (start != _first && start != _last)
            return false;

        if (_points.Count > 1)
        {
            if(start == _first)
            {
                _points.ModeFirst = true;
                SetGradient(0);
            }
            else 
            {
                _points.ModeLast = true;
                SetGradient(^1);
            }
            
            _textureScale.x += _textureScaleX;
        }

        CreateLine(start, end);

        return true;

        #region Local: SetGradient()
        //=================================
        void SetGradient(Index index)
        {
            Gradient gradient = new();
            var alphas = _roadRenderer.colorGradient.alphaKeys;
            alphas[index].alpha = notAlphaEnd ? 1.0f : 0.0f;
            gradient.SetKeys(_roadRenderer.colorGradient.colorKeys, alphas);
            _roadRenderer.colorGradient = gradient;
        }
        #endregion
    }

    public RoadLine Combining(RoadLine other)
    {
        if (_points.Count < other._points.Count)
            return other.Combining(this);

        if (_first != other._first && _last != other._last && _first != other._last && _last != other._first)
            return null;

        var alphas = _roadRenderer.colorGradient.alphaKeys;
        var alphasOther = other._roadRenderer.colorGradient.alphaKeys;

        if (other._first == _last)
        {
            _points.AddFirstToLast(other._points);
            SetGradient(alphas[0].alpha, alphasOther[^1].alpha);
        }
        else if(other._last == _first)
        {
            _points.AddLastToFirst(other._points);
            SetGradient(alphasOther[0].alpha, alphas[^1].alpha);
        }
        else if (_first == other._first)
        {
            _points.AddFirstToFirst(other._points);
            SetGradient(alphasOther[^1].alpha, alphas[^1].alpha);
        }
        else //if (_last == other._last)
        {
            _points.AddLastToLast(other._points);
            SetGradient(alphas[0].alpha, alphasOther[0].alpha);
        }

        SetTextureScale();

        _first = _points.First;
        _last = _points.Last;

        _roadRenderer.positionCount = _points.Count;
        _roadRenderer.SetPositions(_points.ToArray());

        return other;

        #region Local: SetTextureScale(), SetGradient(...)
        //=================================
        void SetTextureScale()
        {
            _countSegments += other._countSegments;
            _textureScale = _textureScale + other._textureScale;
            _textureScale.y *= 0.5f;
            _textureScaleX = _textureScale.x /= _countSegments;
            _textureScale.x += _textureScaleX * (_countSegments - 1);

            _roadRenderer.textureScale = _textureScale;
        }
        //=================================
        void SetGradient(float alfaFirst, float alfaLast)
        {
            Gradient gradient = new();
            alphas[0].alpha = alfaFirst;
            alphas[^1].alpha = alfaLast;
            gradient.SetKeys(_roadRenderer.colorGradient.colorKeys, alphas);
            _roadRenderer.colorGradient = gradient;
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

        _first = _points.First;
        _last = _points.Last;

        _countSegments++;

        _roadRenderer.textureScale = _textureScale;
        _roadRenderer.positionCount = _points.Count;
        _roadRenderer.SetPositions(_points.ToArray());
    }
}
