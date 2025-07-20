using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AMoveUsingLerp : IWait
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed;

        private Vector3 _start, _end, _delta;
        private float _progress = 1f;
        private bool _isRunning;

        protected abstract float DeltaTime { get; }

        public Transform Transform => _transform;

        public object Current => null;
        public bool IsRunning => _isRunning;

        public AMoveUsingLerp(Transform transform, float speed)
        {
            _transform = transform; _speed = speed;
        }

        public bool MoveNext()
        {
            _progress += DeltaTime * _speed;
            if (_isRunning = _progress < 1f)
                _transform.localPosition = new(_start.x + _delta.x * _progress, _start.y + _delta.y * _progress, _start.z + _delta.z * _progress);
            else
                _transform.localPosition = _end;
            
            return _isRunning;
        }

        public IEnumerator Run(Vector3 target)
        {
            Set(_transform.localPosition, target);
            return this;
        }
        public IEnumerator Run(Vector3 current, Vector3 target)
        {
            Set(current, target);
            return this;
        }

        public void Run(MonoBehaviour mono, Vector3 target)
        {
            Set(_transform.localPosition, target);
            if (!_isRunning)
                mono.StartCoroutine(this);
        }

        public void Skip()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _progress = 1f;
                _transform.localPosition = _end;
            }
        }
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _progress = 1f;
            }
        }

        public void Reset() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(Vector3 current, Vector3 target)
        {
            _start    = current; 
            _end      = target; 
            _delta    = _end - _start;
            _progress = 0f;
        }
    }
}
