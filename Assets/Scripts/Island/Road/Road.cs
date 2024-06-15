using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private LineRenderer _roadRenderer;
    [Space]
    [SerializeField] private float _widthRoad = 1.1f;
    [SerializeField] private Vector2Int _rangeCount = new(2, 5);
    [SerializeField] private float _offsetY = 0.05f;
    [SerializeField] private Vector2 _rateWave = new(0.5f, 0.9f);
    [SerializeField] private Vector2 _lengthFluctuation = new(0.85f, 1.15f);
    [Space]
    [SerializeField] private Vector2 _textureScaleMin = new(0.6f, 0.4f);
    [SerializeField] private Vector2 _textureScaleMax = new(0.9f, 1f);
    [Space]
    [SerializeField] private float _alphaTime = 0.01f;

    private readonly LinkList<Crossroad> _crossroads = new();
    private readonly LinkList<Vector3> _points = new();
    private PlayerType _owner;
    private GradientLine _gradient;
    private Vector2 _textureScale;
    private float _textureScaleX;

    public void Initialize(Crossroad start, Crossroad end, Player player)
    {
        _owner = player.Type;
        _crossroads.Add(start, end);

        InitializeLineRenderer(start.Position, player.Color);

        CreateLine(start.Position, end.Position);

        #region Local: InitializeLineRenderer(...)
        //=================================
        void InitializeLineRenderer(Vector3 start, Color color)
        {
            _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;
            _textureScale = URandom.Vector2(_textureScaleMin, _textureScaleMax);
            _textureScaleX = _textureScale.x;

            _gradient = new(_roadRenderer, _alphaTime, color);

            start.y = _offsetY;
            _points.Add(start);
        }
        #endregion
    }

    public bool TryAdd(Crossroad start, Crossroad end)
    {
        if (start == _crossroads.First)
            AddFirst();
        else if (start == _crossroads.Last)
            AddLast();
        else
            return false;

        CreateLine(start.Position, end.Position);

        return true;
        #region Local: AddFirst(), AddLast()
        //=================================
        void AddFirst()
        {
            _crossroads.AddFirst(end);
            _points.Mode = AddMode.First;
        }
        //=================================
        void AddLast()
        {
            _crossroads.AddLast(end);
            _points.Mode = AddMode.Last;
        }
        #endregion
    }

    public Road Union(Road other)
    {
        if (_points.Count < other._points.Count)
            return other.Union(this);

        string s1 = $"{_crossroads.Count} + {other._crossroads.Count - 1} = ";
        string s2 = $"{_points.Count} + {other._points.Count - 1} = ";

        if (!(_crossroads.Union(other._crossroads) && _points.Union(other._points)))
            return null;

        _textureScale = _textureScale + other._textureScale;
        _textureScale.y *= 0.5f;
        _textureScaleX = _textureScale.x / (_crossroads.Count - 1);

        SetLineRenderer();

        return other;
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        start.y = end.y = _offsetY;

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

        SetLineRenderer();
    }

    private void SetLineRenderer()
    {
        _gradient.AlphaFirst = _crossroads.First.IsFullOwned(_owner) ? 1f : 0f;
        _gradient.AlphaLast = _crossroads.Last.IsFullOwned(_owner) ? 1f : 0f;
        _gradient.SetKeys();

        _textureScale.x = _textureScaleX * (_crossroads.Count - 1);

        _roadRenderer.textureScale = _textureScale;
        _roadRenderer.positionCount = _points.Count;
        _roadRenderer.SetPositions(_points.ToArray());
    }

    #region Nested class: GradientLine
    private class GradientLine
    {
        public float AlphaFirst {  get => _alphas[0].alpha; set => _alphas[0].alpha = value; }
        public float AlphaLast { get => _alphas[^1].alpha; set => _alphas[^1].alpha = value; }

        private readonly LineRenderer _lineRenderer;
        private readonly Gradient _gradient = new();

        private readonly GradientAlphaKey[] _alphas;
        private readonly GradientColorKey[] _colors;

        public GradientLine(LineRenderer lineRenderer, float alphaTime, Color color)
        {
            _lineRenderer = lineRenderer;

            _alphas = new GradientAlphaKey[] { new(0.0f, 0.0f), new(1.0f, alphaTime), new(1.0f, 1f - alphaTime), new(0.0f, 1.0f) };
            _colors = new GradientColorKey[] { new(color, 0.0f), new(color, 1.0f) };

            SetKeys();
        }

        public void SetKeys()
        {
            _gradient.SetKeys(_colors, _alphas);
            _lineRenderer.colorGradient = _gradient;
        }
    }
    #endregion
}