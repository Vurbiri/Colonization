using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class RunAll : Enumerator
    {
        private readonly Dictionary<int, Coroutine> _coroutines = new();
        private readonly MonoBehaviour _mono;
        private int _counter;

        public int Count { [Impl(256)] get => _coroutines.Count; }
        public bool IsRunning { [Impl(256)] get => _coroutines.Count > 0; }

        [Impl(256)] public RunAll() => _mono = CoroutineInternal.Instance;
        [Impl(256)] public RunAll(MonoBehaviour mono) => _mono = mono;

        public IEnumerator Start(IEnumerator routine)
        {
            Run(routine);
            return this;
        }
        public IEnumerator Start(IEnumerator routine1, IEnumerator routine2)
        {
            Run(routine1); Run(routine2);
            return this;
        }
        public IEnumerator Start(IEnumerator routine1, IEnumerator routine2, IEnumerator routine3)
        {
            Run(routine1); Run(routine2); Run(routine3);
            return this;
        }
        public IEnumerator Start(params IEnumerator[] routines)
        {
            for (int i = routines.Length - 1; i >= 0; --i)
                Run(routines[i]);
            return this;
        }
        public IEnumerator Start(IEnumerable<IEnumerator> routines)
        {
            foreach (IEnumerator coroutine in routines)
                Run(coroutine);
            return this;
        }

        public void Stop()
        {
            foreach(var coroutine in _coroutines.Values)
                _mono.StopCoroutine(coroutine);

            _coroutines.Clear();
        }

        public override bool MoveNext() => _coroutines.Count > 0;

        [Impl(256)] private void Run(IEnumerator routine)
        {
            int id = _counter++;
            var coroutine = _mono.StartCoroutine(Run_Cn(routine, id));
            _coroutines.Add(id, coroutine);
        }

        private IEnumerator Run_Cn(IEnumerator routine, int id)
        {
            yield return routine;
            _coroutines.Remove(id);
        }
    }
}
