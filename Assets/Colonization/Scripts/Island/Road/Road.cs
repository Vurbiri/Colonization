using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Road : MonoBehaviour
    {
        [GetComponentInChildren]
        [SerializeField] private LineRenderer _roadRenderer;
        [Space]
        [SerializeField] private float _widthRoad = 1.1f;
        [SerializeField] private RInt _rangeCount = new(2, 4);
        [SerializeField] private float _offsetY = 0.05f;
        [SerializeField] private RFloat _rateWave = new(0.5f, 0.9f);
        [SerializeField] private RFloat _lengthFluctuation = new(0.85f, 1.15f);
        [Space]
        [SerializeField] private RFloat _textureXRange = new(0.6f, 0.9f);
        [SerializeField] private RFloat _textureYRange = new(0.4f, 1f);
        [Space]
        [SerializeField] private float _alphaTime = 0.01f;

        private readonly LinkList<Crossroad> _crossroads = new();
        private readonly LinkList<Vector3> _points = new();
        private PlayerType _owner;
        private GradientLine _gradient;
        private Vector2 _textureScale;
        private float _textureScaleX;

        public void Create(Crossroad start, Crossroad end, PlayerType type, Color color)
        {
            _owner = type;
            _crossroads.Add(start, end);

            InitializeLineRenderer(start.Position, color);

            CreateLine(start.Position, end.Position);

            #region Local: InitializeLineRenderer(...)
            //=================================
            void InitializeLineRenderer(Vector3 start, Color color)
            {
                _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;
                _textureScale = new Vector2(_textureXRange, _textureYRange);
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
                _points.Mode = LinkListMode.First;
            }
            //=================================
            void AddLast()
            {
                _crossroads.AddLast(end);
                _points.Mode = LinkListMode.Last;
            }
            #endregion
        }

        public Road Union(Road other)
        {
            if (_points.Count < other._points.Count)
                return other.Union(this);

            if (!(_crossroads.Union(other._crossroads) && _points.Union(other._points)))
                return null;

            _textureScale = _textureScale + other._textureScale;
            _textureScale.y *= 0.5f;
            _textureScaleX = _textureScale.x / (_crossroads.Count - 1);

            SetLineRenderer();

            return other;
        }

        public Key[] GetCrossroadsKey()
        {
            Key[] keys = new Key[_crossroads.Count];
            int i = 0;
            foreach (var crossroad in _crossroads)
                keys[i++] = crossroad.Key;

            return keys;
        }

        private void CreateLine(Vector3 start, Vector3 end)
        {
            start.y = end.y = _offsetY;

            int count = _rangeCount;
            RFloat wave = new(_rateWave, _widthRoad / count);
            Vector3 step = (end - start) / (count + 1), offsetSide = Vector3.Cross(Vector3.up, step.normalized);
            float sign = Chance.Select(1f, -1f), signStep = -1f;

            for (int i = 0; i < count; i++)
            {
                sign *= signStep;
                start += _lengthFluctuation * step + wave * sign * offsetSide;
                _points.Add(start);
            }
            _points.Add(end);

            SetLineRenderer();
        }

        public void SetGradient()
        {
            _gradient.AlphaFirst = _crossroads.First.IsFullyOwned(_owner) ? 1f : 0f;
            _gradient.AlphaLast = _crossroads.Last.IsFullyOwned(_owner) ? 1f : 0f;
            _gradient.SetKeys();
        }

        private void SetLineRenderer()
        {
            SetGradient();

            _textureScale.x = _textureScaleX * (_crossroads.Count - 1);

            _roadRenderer.textureScale = _textureScale;
            _roadRenderer.positionCount = _points.Count;
            _roadRenderer.SetPositions(_points.ToArray());
        }

        #region Nested class: GradientLine
        private class GradientLine
        {
            public float AlphaFirst { get => _alphas[0].alpha; set => _alphas[0].alpha = value; }
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
}
