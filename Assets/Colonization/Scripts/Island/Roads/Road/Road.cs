using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Road : MonoBehaviour
    {
        private const int BASE_ORDER = 32;
        private const int R_SFX_COUNT = 2;
        private const float CLIP_DELAY = 0.5f;

        private static Coroutine s_playRemoveClip;

        [SerializeField] private LineRenderer _roadRenderer;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _removeClip;
        [SerializeField] private WaitParticle[] _removeSFX;
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
        [SerializeField, Range(2f, 6f)] private float _buildingSpeed = 4.3f;

        private readonly Subscription<Road> _onDisable = new();
        private readonly WaitRealtime _waitClip = new(CLIP_DELAY);
        private readonly WaitSignal _waitSignal = new();
        private Transform _thisTransform;
        private readonly Transform[] _particleTransforms = new Transform[R_SFX_COUNT];
        private Links _links;
        private Points _points;
        private Vector2 _textureScale;
        private float _textureScaleX;
                
        private Gradient _gradient;
        private Coroutine _coroutineBuildSFX, _coroutineRemoveSFX;
        private readonly GradientAlphaKey[] _alphaKeys = { new(1f, 0f), new(1f, 0.5f), new(0f, 1f) };
        private GradientAlphaKey[] _defaultAlphaKey;
        private GradientAlphaKey[] LineAlphaKeys { set { _gradient.alphaKeys = value; _roadRenderer.colorGradient = _gradient; } }

        public Transform Transform => _thisTransform;

        public int SortingOrder { set =>  _roadRenderer.sortingOrder = BASE_ORDER - value; }
        public int Count => _links.Count;

        public Road Init(Gradient gradient, int id, Action<Road> onDisable)
        {
            Setup(gradient, id);

            _onDisable.Add(onDisable);
            _thisTransform = transform;
            for (int i = 0; i < R_SFX_COUNT; i++)
                _particleTransforms[i] = _removeSFX[i].ParticleSystem.transform;

            return this;
        }

        public Road Setup(Gradient gradient, int id, Transform parent)
        {
            _thisTransform.SetParent(parent, false);

            Setup(gradient, id);

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

        public bool ThereDeadEnds(int playerId) => _links.End.IsDeadEnd(playerId) || _links.Start.IsDeadEnd(playerId);
        public int DeadEndsCount(int playerId)
        {
            int deadEndsCount = 0;
            if (_links.End.IsDeadEnd(playerId))   deadEndsCount++;
            if (_links.Start.IsDeadEnd(playerId)) deadEndsCount++;
            return deadEndsCount;
        }
        public int RemoveDeadEnds(int playerId)
        {
            int removeCount = 0;
            if (_links.End.IsDeadEnd(playerId, out CrossroadLink link))
            {
                _links.Remove();
                _points.Remove(_links.End.Position);
                RoadRemove(link, removeCount++);
            }
            if (_links.Start.IsDeadEnd(playerId, out link))
            {
                _links.Extract();
                _points.Extract(_links.Start.Position);
                RoadRemove(link, removeCount++);
            }

            if (removeCount > 0)
                SetTextureScale();

            return removeCount;
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

        public void Disable()
        {
            _points.Clear();
            _onDisable.Invoke(this);
        }

        private void RoadRemove(CrossroadLink link, int index)
        {
            link.RoadRemove();
            _particleTransforms[index].SetLocalPositionAndRotation(link.Position, CONST.LINK_ROTATIONS[link.Id.Value]);
            _removeSFX[index].Play();

            _coroutineRemoveSFX ??= StartCoroutine(RemoveSFX_Cn());
            s_playRemoveClip ??= StartCoroutine(PlayRemoveClip_Cn());
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
            if(_coroutineBuildSFX != null)
            {
                StopCoroutine(_coroutineBuildSFX);
                StopSFX();
            }
            _coroutineBuildSFX = StartCoroutine(BuildSFX_Cn(inverse));
            return _waitSignal.Restart();
        }

        private void StopSFX()
        {
            LineAlphaKeys = _defaultAlphaKey;
            _waitSignal.Send();
            _coroutineBuildSFX = null;
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

        private IEnumerator RemoveSFX_Cn()
        {
            for (int i = 0; i < R_SFX_COUNT; i++)
                yield return _removeSFX[i];

            _coroutineRemoveSFX = null;

            if (_links.Count < 2)
                Disable();
        }

        private IEnumerator PlayRemoveClip_Cn()
        {
            _audioSource.PlayOneShot(_removeClip);
            yield return _waitClip.Restart();
            s_playRemoveClip = null;
        }

        private void Setup(Gradient gradient, int id)
        {
            _rateWave = new(_rateWave, _widthRoad);

            _roadRenderer.sortingOrder = BASE_ORDER - id;

            _roadRenderer.startWidth = _roadRenderer.endWidth = _widthRoad;

            _roadRenderer.colorGradient = gradient;
            _defaultAlphaKey = gradient.alphaKeys;
            _gradient = _roadRenderer.colorGradient;

            _textureScale = new Vector2(_textureXRange, _textureYRange);
            _textureScaleX = _textureScale.x;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _audioSource);
            this.SetChildren(ref _roadRenderer);

            if(_removeSFX == null || _removeSFX.Length != R_SFX_COUNT)
            {
                var particles = GetComponentsInChildren<ParticleSystem>();
                _removeSFX = new WaitParticle[R_SFX_COUNT];
                for (int i = 0; i < R_SFX_COUNT; i++)
                {
                    _removeSFX[i] = new(particles[i]);
                    var shape = particles[i].shape;
                    shape.radius = CONST.HEX_RADIUS_OUT * 0.5f;
                }
            }
        }
#endif
    }
}
