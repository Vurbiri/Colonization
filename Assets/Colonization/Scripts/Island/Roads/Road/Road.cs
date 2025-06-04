using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Road : MonoBehaviour
    {
        private const int BASE_ORDER = 32;

        [SerializeField] private LineRenderer _roadRenderer;
        [SerializeField] private AudioSource _audioSource;
        [Space]
        [SerializeField, Range(0.5f, 1.5f)] private float _widthRoad = 0.95f;
        [Space]
        [SerializeField, MinMax(2, 7)] private IntRnd _rangeCount = new(3, 6);
        [SerializeField, MinMax(0.5f, 1.5f)] private FloatRnd _lengthFluctuation = new(0.85f, 1.15f);
        [SerializeField, MinMax(0.1f, 0.32f)] private FloatRnd _rateWave = new(0.19f, 0.27f);
        [Space]
        [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _textureXRange = new(0.6f, 0.9f);
        [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _textureYRange = new(0.4f, 1f);
        [Space]
        [SerializeField, Range(2f, 6f)] private float _buildingSpeed = 4.1f;

        private readonly WaitSignal _waitSignal = new();
        private Links _links;
        private Points _points;
        private Vector2 _textureScale;
        private float _textureScaleX;

        private Gradient _gradient;
        private Coroutine _coroutineSFX;
        private readonly GradientAlphaKey[] _alphaKeys = { new(1f, 0f), new(1f, 0.5f), new(0f, 1f) };
        private GradientAlphaKey[] _defaultAlphaKey;

        private GradientAlphaKey[] LineAlphaKeys { set { _gradient.alphaKeys = value; _roadRenderer.colorGradient = _gradient; } }

        public int SortingOrder { set =>  _roadRenderer.sortingOrder = BASE_ORDER - value; }
        public int Count => _links.Count;

        public Road Init(Gradient gradient, int id)
        {
            _rateWave = new(_rateWave, _widthRoad);

            _roadRenderer.sortingOrder = BASE_ORDER - id;

            _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;

            _roadRenderer.colorGradient = gradient;
            _defaultAlphaKey = gradient.alphaKeys;
            _gradient = _roadRenderer.colorGradient;

            _textureScale = new Vector2(_textureXRange, _textureYRange);
            _textureScaleX = _textureScale.x;

            return this;
        }

        public ReturnSignal Create(Crossroad start, Crossroad end, bool isSFX)
        {
            _links = new(start, end);
            _points = new(this, _roadRenderer, start.Position, end.Position);
            SetTextureScale();

            return isSFX ? StartSFX() : true;
        }

        public ReturnSignal TryAdd(Crossroad start, Crossroad end, bool isSFX)
        {
            bool inverse;

            if (start ==_links.End)
            {
                _links.Add(end);
                _points.Add(start.Position, end.Position);
                inverse = false;
            }
            else if (start == _links.Start)
            {
                _links.Insert(end);
                _points.Insert(start.Position, end.Position);
                inverse = true;
            }
            else
            {
                return false;
            }

            SetTextureScale();

            return isSFX ? StartSFX(inverse) : true;
        }

        public Road Union(Road other)
        {
            Crossroad selfEnd = _links.End, otherStart = other._links.Start;
            if (selfEnd == otherStart)
            {
                _links.AddRange(other._links);
                _points.AddRange(other._points);
                UnionTextureScale(other);
                return other;
            }

            Crossroad selfStart = _links.Start, otherEnd = other._links.End;
            if (selfEnd == otherEnd)
            {
                _links.AddReverseRange(other._links);
                _points.AddReverseRange(other._points);
                UnionTextureScale(other);
                return other;
            }

            if (selfStart == otherEnd | selfStart == otherStart)
            {
                return other.Union(this);
            }

            return null;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void UnionTextureScale(Road other)
        {
            _textureScale = _textureScale + other._textureScale;
            _textureScale.y *= 0.5f;
            _textureScaleX = _textureScale.x / (_links.Count - 1);

            SetTextureScale();
        }

        private void SetTextureScale()
        {
            _textureScale.x = _textureScaleX * (_links.Count - 1);
            _roadRenderer.textureScale = _textureScale;
        }

        private WaitSignal StartSFX(bool inverse = false)
        {
            if(_coroutineSFX != null)
            {
                StopCoroutine(_coroutineSFX);
                StopSFX();
            }
            _coroutineSFX = StartCoroutine(BuildSFX_Cn(inverse));
            return _waitSignal.Restart();
        }

        private void StopSFX()
        {
            LineAlphaKeys = _defaultAlphaKey;
            _waitSignal.Send();
            _coroutineSFX = null;
        }

        private IEnumerator BuildSFX_Cn(bool inverse)
        {
            float count = _links.Count;
            if (count <= 1f) yield break;

            float start = (count - 2f) / (count - 1f), end = 1f;
            if(inverse)
            {
                start = 1f - start; end = 0f;
            }
            float progress = 0f;

            _alphaKeys[0] .alpha = end;
            _alphaKeys[^1].alpha = 1f - end;

            _audioSource.Play();
            while (progress < 1f)
            {
                _alphaKeys[1].time = Mathf.Lerp(start, end, progress);
                LineAlphaKeys = _alphaKeys;
                progress += Time.unscaledDeltaTime * _buildingSpeed;
                yield return null;
            }

            StopSFX();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_roadRenderer == null)
                _roadRenderer = GetComponentInChildren<LineRenderer>();
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (Application.isPlaying) return;

            if (_audioSource.playOnAwake)
                _audioSource.playOnAwake = false;
            if (_audioSource.loop)
                _audioSource.loop = false;
        }
#endif
    }
}
