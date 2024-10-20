using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public class WaitAll : CustomYieldInstruction
    {
        private readonly HashSet<IEnumerator> _coroutines = new();
        private readonly MonoBehaviour _monoBehaviour;

        public override bool keepWaiting => _coroutines.Count != 0;

        public WaitAll(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }
        public WaitAll(MonoBehaviour monoBehaviour, IEnumerator coroutine)
        {
            _monoBehaviour = monoBehaviour;
            Add(coroutine);
        }
        public WaitAll(MonoBehaviour monoBehaviour, params IEnumerator[] coroutines)
        {
            _monoBehaviour = monoBehaviour;
            AddRange(coroutines);
        }

        public WaitAll Add(IEnumerator coroutine)
        {
            _monoBehaviour.StartCoroutine(AddCoroutine(coroutine));
            return this;
        }
        public WaitAll AddRange(params IEnumerator[] coroutines)
        {
            foreach (var coroutine in coroutines)
                Add(coroutine);

            return this;
        }

        public void Stop()
        {
            foreach (var coroutine in _coroutines)
                _monoBehaviour.StopCoroutine(coroutine);

            _coroutines.Clear();
        }

        private IEnumerator AddCoroutine(IEnumerator coroutine)
        {
            _coroutines.Add(coroutine);
            yield return coroutine;
            _coroutines.Remove(coroutine);
        }

    }
}
