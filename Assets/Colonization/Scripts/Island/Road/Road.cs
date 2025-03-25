//Assets\Colonization\Scripts\Island\Road\Road.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Road : MonoBehaviour
    {
        [SerializeField] private LineRenderer _roadRenderer;
        [Space]
        [SerializeField, Range(0.5f, 1.5f)] private float _widthRoad = 0.95f;
        [Space]
        [SerializeField] private float _offsetY = 0.0125f;
        [Space]
        [SerializeField, MinMax(1, 6)] private IntRnd _rangeCount = new(2, 5);
        [SerializeField, MinMax(0.1f, 0.3f)] private FloatRnd _rateWave = new(0.12f, 0.24f);
        [SerializeField, MinMax(0.5f, 1.5f)] private FloatRnd _lengthFluctuation = new(0.85f, 1.15f);
        [Space]
        [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _textureXRange = new(0.6f, 0.9f);
        [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _textureYRange = new(0.4f, 1f);

        private readonly LinkList<Crossroad> _crossroads = new();
        private readonly LinkList<Vector3> _points = new();
        private Vector2 _textureScale;
        private float _textureScaleX;

        public Road Init(Crossroad start, Crossroad end, Gradient gradient)
        {
            _rateWave = new(_rateWave, _widthRoad);


            _crossroads.Add(start, end);

            InitLineRenderer(start.Position, gradient);
            CreateLine(start.Position, end.Position);

            return this;

            #region Local: InitLineRenderer(...)
            //=================================
            void InitLineRenderer(Vector3 start, Gradient gradient)
            {
                _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;
                _roadRenderer.colorGradient = gradient;
                _textureScale = new Vector2(_textureXRange, _textureYRange);
                _textureScaleX = _textureScale.x;
                
                start.y = _offsetY;
                _points.Add(start);
            }
            #endregion
        }

        public bool TryAdd(Crossroad start, Crossroad end)
        {
            if (start == _crossroads.First)
                AddFirst(end);
            else if (start == _crossroads.Last)
                AddLast(end);
            else
                return false;

            CreateLine(start.Position, end.Position);

            return true;
            #region Local: AddFirst(), AddLast()
            //=================================
            void AddFirst(Crossroad end)
            {
                _crossroads.AddToFirst(end);
                _points.Mode = LinkListMode.First;
            }
            //=================================
            void AddLast(Crossroad end)
            {
                _crossroads.AddToLast(end);
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

        private void CreateLine(Vector3 start, Vector3 end)
        {
            start.y = end.y = _offsetY;

            int count = _rangeCount;
            Vector3 step = (end - start) / (count + 1), offsetSide = Vector3.Cross(Vector3.up, step.normalized);
            float sign = Chance.Select(1f, -1f), signStep = -1f;

            for (int i = 0; i < count; i++)
            {
                sign *= signStep;
                start += _lengthFluctuation * step + _rateWave * sign * offsetSide;
                _points.Add(start);
            }
            _points.Add(end);

            SetLineRenderer();
        }

        private void SetLineRenderer()
        {
            _textureScale.x = _textureScaleX * (_crossroads.Count - 1);

            _roadRenderer.textureScale = _textureScale;
            _roadRenderer.positionCount = _points.Count;
            _roadRenderer.SetPositions(_points.ToArray());
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_roadRenderer == null)
                _roadRenderer = GetComponentInChildren<LineRenderer>();
        }
#endif

    }
}
