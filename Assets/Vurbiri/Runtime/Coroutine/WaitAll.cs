using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    public class WaitAll : IWait
    {
        private readonly List<IEnumerator> _coroutines;

        public object Current => null;
        public int Count => _coroutines.Count;
        public bool IsWait => _coroutines.Count != 0;

        public WaitAll()
        {
            _coroutines = new();
        }

        public WaitAll(IEnumerator coroutine)
        {
            _coroutines = new() { coroutine };
        }
        public WaitAll(IEnumerator coroutine1, IEnumerator coroutine2)
        {
            _coroutines = new() { coroutine1, coroutine2 };
        }
        public WaitAll(params IEnumerator[] coroutines)
        {
            _coroutines = new(coroutines);
        }
        public WaitAll(IEnumerable<IEnumerator> coroutines)
        {
            _coroutines = new(coroutines);
        }

        public bool MoveNext()
        {
            for (int i = _coroutines.Count - 1; i >= 0; i--)
                if (!_coroutines[i].MoveNext())
                    _coroutines.RemoveAt(i);

            return _coroutines.Count != 0;
        }

        public IEnumerator Add(IEnumerator coroutine)
        {
            _coroutines.Add(coroutine);
            return this;
        }
        public IEnumerator Add(IEnumerator coroutine1, IEnumerator coroutine2)
        {
            _coroutines.Add(coroutine1);
            _coroutines.Add(coroutine2);
            return this;
        }
        public IEnumerator Add(params IEnumerator[] coroutines)
        {
            _coroutines.AddRange(coroutines);
            return this;
        }
        public IEnumerator AddRange(IEnumerable<IEnumerator> coroutines)
        {
            _coroutines.AddRange(coroutines);
            return this;
        }

        public void Clear()
        {
            _coroutines.Clear();
        }

        public void Reset() { }
    }
}
