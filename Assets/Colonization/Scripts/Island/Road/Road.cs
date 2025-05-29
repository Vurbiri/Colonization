using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Road : MonoBehaviour
    {
        [SerializeField] private LineRenderer _roadRenderer;
        [Space]
        [SerializeField, Range(0.5f, 1.5f)] private float _widthRoad = 0.95f;
        [Space]
        //[SerializeField] private float _offsetY = 0.0125f;
        //[SerializeField, MinMax(2, 7)] private IntRnd _rangeCount = new(3, 6);
        //[SerializeField, MinMax(0.5f, 1.5f)] private FloatRnd _lengthFluctuation = new(0.85f, 1.15f);
        [SerializeField, MinMax(0.1f, 0.3f)] private FloatRnd _rateWave = new(0.12f, 0.24f);
        [Space]
        [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _textureXRange = new(0.6f, 0.9f);
        [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _textureYRange = new(0.4f, 1f);
        [Space]
        [SerializeField, Range(0.1f, 10f)] private float _buildingSpeed = 1.5f;

        private readonly WaitSignal _waitSignal = new();
        private List<Key> _keys = new(16);
        private Points _points;
        private Vector2 _textureScale;
        private float _textureScaleX;

        private Gradient _gradient;
        private Coroutine _coroutineSFX;
        private readonly GradientAlphaKey[] _alphaKeys = { new(1f, 0f), new(1f, 0.5f), new(0f, 1f) };
        private GradientAlphaKey[] _defaultAlphaKey;

        private GradientAlphaKey[] LineAlphaKeys { set { _gradient.alphaKeys = value; _roadRenderer.colorGradient = _gradient; } }

        public Road Init(Gradient gradient)
        {
            _rateWave = new(_rateWave, _widthRoad);

            _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;
            _roadRenderer.colorGradient = gradient;
            _defaultAlphaKey = gradient.alphaKeys;
            _gradient = gradient;
            _textureScale = new Vector2(_textureXRange, _textureYRange);
            _textureScaleX = _textureScale.x;

            return this;
        }

        public ReturnSignal CreateFirst(Crossroad start, Crossroad end, bool isSFX)
        {
            _keys.Add(start.Key); _keys.Add(end.Key);

            _points = new(_roadRenderer, new(_rateWave, _widthRoad), start.Position, end.Position);
            SetTextureScale();

            return isSFX ? StartSFX(LinkMode.Add) : true;
        }

        public ReturnSignal TryAdd(Crossroad start, Crossroad end, bool isSFX)
        {
            LinkMode mode;

            if (start.Equals(_keys[^1]))
            {
                _keys.Add(end.Key);
                _points.Add(start.Position, end.Position);
                mode = LinkMode.Add;
            }
            else if (start.Equals(_keys[0]))
            {
                _keys.Insert(0, end.Key);
                _points.Insert(start.Position, end.Position);
                mode = LinkMode.Insert;
            }
            else
            {
                return false;
            }

            SetTextureScale();

            return isSFX ? StartSFX(mode) : true;
        }

        public Road Union(Road other)
        {
            if (!(Union(other._keys) && _points.Union(other._points)))
                return null;

            _textureScale = _textureScale + other._textureScale;
            _textureScale.y *= 0.5f;
            _textureScaleX = _textureScale.x / (_keys.Count - 1);

             SetTextureScale();

             return other;
        }

        private void SetTextureScale()
        {
            _textureScale.x = _textureScaleX * (_keys.Count - 1);
            _roadRenderer.textureScale = _textureScale;
        }

        private WaitSignal StartSFX(LinkMode mode)
        {
            if(_coroutineSFX != null)
            {
                StopCoroutine(_coroutineSFX);
                StopSFX();
            }
            _coroutineSFX = StartCoroutine(BuildSFX_Cn(mode));
            return _waitSignal;
        }

        private void StopSFX()
        {
            LineAlphaKeys = _defaultAlphaKey;
            _waitSignal.Send();
            _coroutineSFX = null;
        }

        private IEnumerator BuildSFX_Cn(LinkMode mode)
        {
            float count = _keys.Count;
            if (count <= 1f) yield break;

            float start = (count - 2f) / (count - 1f), end = 1f;
            if(mode == LinkMode.Insert)
            {
                start = 1f - start; end = 0f;
            }
            float progress = 0f;

            _alphaKeys[0] .alpha = end;
            _alphaKeys[^1].alpha = 1f - end;
            while (progress < 1f)
            {
                _alphaKeys[1].time = Mathf.Lerp(start, end, progress);
                LineAlphaKeys = _alphaKeys;
                progress += Time.deltaTime * _buildingSpeed;
                yield return null;
            }

            StopSFX();
        }

        private bool Union(List<Key> other)
        {
            int selfEndIndex = _keys.Count - 1;
            Key selfEnd = _keys[selfEndIndex], otherZero = other[0];
            if (selfEnd == otherZero)
            {
                _keys.RemoveAt(selfEndIndex);
                _keys.AddRange(other);
                return true;
            }

            int otherEndIndex = other.Count - 1;
            Key selfZero = _keys[0], otherEnd = other[otherEndIndex];
            if (selfZero == otherEnd)
            {
                other.RemoveAt(otherEndIndex);
                other.AddRange(_keys);
                _keys = other;
                return true;
            }

            if (selfEnd == otherEnd)
            {
                other.RemoveAt(otherEndIndex);
                other.Reverse();
                _keys.AddRange(other);
                return true;
            }

            if (selfZero == otherZero)
            {
                _keys.Reverse();
                _keys.RemoveAt(selfEndIndex);
                other.AddRange(_keys);
                _keys = other;
                return true;
            }

            return false;
        }

        private enum LinkMode
        {
            Add,
            Insert
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
