using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri
{
    [Serializable]
    public abstract class AWaitSwitchFloat : IEnumerator
    {
        [SerializeField] private float _min, _max;
        [SerializeField] private float _speed;
        [SerializeField] private Listener<float> _setMethod;

        private Action<float> _update;
        private float _start, _end, _delta;
        private float _progress;

        protected abstract float DeltaTime { get; }

        public object Current => null;
        public bool IsRunning => _progress < 1f;
        public float Speed { get => _speed; set => _speed = value; }

        public AWaitSwitchFloat(float min, float max, float speed, Action<float> update)
        {
            _min = min; _max = max; _speed = speed;
            _update = update; _setMethod = null;
        }
        public void Init()
        {
            _update = _setMethod.CreateDelegate(); _setMethod = null;
        }
        public void Init(Action<float> update)
        {
            _update = update; _setMethod = null;
        }

        public bool MoveNext()
        {
            _progress += DeltaTime * _speed;
            if (_progress < 1f)
            {
                _update(_start + _delta * _progress);
                return true;
            }

            _update(_end);
            return false;
        }

        public IEnumerator Forward() => Set(_min, _max, 0f);
        public IEnumerator Forward(float current) => Set(_min, _max, current - _min);

        public IEnumerator Backward() => Set(_max, _min, 0f);
        public IEnumerator Backward(float current) => Set(_max, _min, current - _max);

        public void Reset()
        {
            _progress = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator Set(float start, float end, float progress)
        {
            _start = start; _end = end; _delta = end - start;
            _progress = progress / _delta;
            return this;
        }

#if UNITY_EDITOR
        public void OnValidate(float min, float max, Action<float> update)
        {
            _min = min; _max = max;
            _setMethod = new(update);
            if (_speed < 0.01f) _speed = 0.01f;
        }
        public bool IsValid_Editor => _max > _min & _setMethod != null && _setMethod.IsValid;
#endif
    }
}
