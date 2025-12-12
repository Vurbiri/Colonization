using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	sealed public class WaitAll : AWait, System.IDisposable
    {
        private readonly Dictionary<int, Coroutine> _coroutines = new();
        private readonly MonoBehaviour _mono;
        private int _counter = -1;

        public int Count { [Impl(256)] get => _coroutines.Count; }
        public bool IsRunning { [Impl(256)] get => _coroutines.Count > 0; }

        [Impl(256)] public WaitAll() => _mono = CoroutineInternal.Instance;
        [Impl(256)] public WaitAll(MonoBehaviour mono) => _mono = mono;

        #region Add IEnumerator
        public IEnumerator Add(IEnumerator routine)
        {
            Run(routine);
            return this;
        }
        public IEnumerator Add(IEnumerator routine1, IEnumerator routine2)
        {
            Run(routine1); Run(routine2);
            return this;
        }
        public IEnumerator Add(IEnumerator routine1, IEnumerator routine2, IEnumerator routine3)
        {
            Run(routine1); Run(routine2); Run(routine3);
            return this;
        }
        public IEnumerator Add(params IEnumerator[] routines)
        {
            for (int i = routines.Length - 1; i >= 0; --i)
                Run(routines[i]);
            return this;
        }
        public IEnumerator Add(IEnumerable<IEnumerator> routines)
        {
            foreach (var coroutine in routines)
                Run(coroutine);
            return this;
        }
        #endregion

        #region Add YieldInstruction
        public IEnumerator Add(YieldInstruction routine)
        {
            Run(routine);
            return this;
        }
        public IEnumerator Add(YieldInstruction routine1, YieldInstruction routine2)
        {
            Run(routine1); Run(routine2);
            return this;
        }
        public IEnumerator Add(YieldInstruction routine1, YieldInstruction routine2, YieldInstruction routine3)
        {
            Run(routine1); Run(routine2); Run(routine3);
            return this;
        }
        public IEnumerator Add(params YieldInstruction[] routines)
        {
            for (int i = routines.Length - 1; i >= 0; --i)
                Run(routines[i]);
            return this;
        }
        public IEnumerator Add(IEnumerable<YieldInstruction> routines)
        {
            foreach (var coroutine in routines)
                Run(coroutine);
            return this;
        }
        #endregion

        [Impl(256)] public void Stop() => Dispose();

        public override bool MoveNext() => _coroutines.Count > 0;

        public void Dispose()
        {
            foreach (var coroutine in _coroutines.Values)
                _mono.StopCoroutine(coroutine);

            _coroutines.Clear();
        }

        [Impl(256)] private void Run<T>(T routine)
        {
            int id = unchecked(++_counter);
            var coroutine = _mono.StartCoroutine(Run_Cn(routine, id));
            _coroutines.Add(id, coroutine);
        }
        private IEnumerator Run_Cn<T>(T routine, int id)
        {
            yield return routine;
            _coroutines.Remove(id);
        }
    }
}
