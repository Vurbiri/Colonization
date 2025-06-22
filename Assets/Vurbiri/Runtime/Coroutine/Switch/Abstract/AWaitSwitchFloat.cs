using System;
using System.Collections;

namespace Vurbiri
{
	public abstract class AWaitSwitchFloat : IEnumerator
    {
        private readonly Action<float> _update;
        private readonly float _speed;
        private readonly float _min, _max;
        private float _start, _end;
        private float _progress;

        protected abstract float DeltaTime { get; }

        public object Current => null;
        public bool IsRunning => _progress < 1f;

        public AWaitSwitchFloat(float min, float max, float speed, Action<float> update)
        {
            _min = min;
            _max = max;
            _speed = speed;
            _update = update;
        }

        public bool MoveNext()
        {
            _progress += DeltaTime * _speed;

            if (_progress < 1f)
            {
                _update(_start + (_end - _start) * _progress);
                return true;
            }

            _update(_end);
            return false;
        }

        public IEnumerator Forward()
        {
            _start = _min; _end = _max;
            _progress = 0f;
            return this;
        }
        public IEnumerator Forward(float current)
        {
            _start = _min; _end = _max;
            _progress = (current - _start) / (_end - _start);
            return this;
        }

        public IEnumerator Backward()
        {
            _start = _max; _end = _min;
            _progress = 0;
            return this;
        }
        public IEnumerator Backward(float current)
        {
            _start = _max; _end = _min;
            _progress = (current - _start) / (_end - _start);
            return this;
        }

        public void Reset()
        {
            _progress = 0f;
        }
    }
}
