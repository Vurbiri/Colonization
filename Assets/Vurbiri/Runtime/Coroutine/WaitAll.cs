using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    public class WaitAll : IEnumerator
    {
        private readonly List<IEnumerator> _coroutines;

        public int Count => _coroutines.Count;
        public object Current => null;

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

        public WaitAll Add(IEnumerator coroutine)
        {
            _coroutines.Add(coroutine);
            return this;
        }
        public WaitAll Add(IEnumerator coroutine1, IEnumerator coroutine2)
        {
            _coroutines.Add(coroutine1);
            _coroutines.Add(coroutine2);
            return this;
        }
        public WaitAll Add(params IEnumerator[] coroutines)
        {
            _coroutines.AddRange(coroutines);
            return this;
        }
        public WaitAll AddRange(IEnumerable<IEnumerator> coroutines)
        {
            _coroutines.AddRange(coroutines);
            return this;
        }

        public void Reset()
        {
            _coroutines.Clear();
        }
    }
}
