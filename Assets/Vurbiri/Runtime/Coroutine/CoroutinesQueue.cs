//Assets\Vurbiri\Runtime\Coroutine\WaitQueue.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public class CoroutinesQueue
    {
        private readonly Queue<IEnumerator> _coroutines = new();
        private readonly MonoBehaviour _monoBehaviour;
        private readonly Action finalAction;
        private Coroutine _runningCoroutine = null;

        public CoroutinesQueue(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }
        public CoroutinesQueue(MonoBehaviour monoBehaviour, Action finalAction)
        {
            _monoBehaviour = monoBehaviour;
            this.finalAction = finalAction;
        }
        public CoroutinesQueue(MonoBehaviour monoBehaviour, IEnumerator coroutine)
        {
            _monoBehaviour = monoBehaviour;
            Enqueue(coroutine);
        }
        public CoroutinesQueue(MonoBehaviour monoBehaviour, Action finalAction, IEnumerator coroutine)
        {
            _monoBehaviour = monoBehaviour;
            this.finalAction = finalAction;
            Enqueue(coroutine);
        }

        public void Enqueue(IEnumerator coroutine)
        {
            _coroutines.Enqueue(coroutine);
            _runningCoroutine ??= _monoBehaviour.StartCoroutine(Run_Coroutine());
        }

        public void StopAndClear(bool runFinalAction)
        {
            if (_runningCoroutine != null)
            {
                _monoBehaviour.StopCoroutine(_runningCoroutine);
                _runningCoroutine = null;
            }

            _coroutines.Clear();

            if(runFinalAction)
                finalAction?.Invoke();
        }

        private IEnumerator Run_Coroutine()
        {
            while (_coroutines.Count > 0)
                yield return _coroutines.Dequeue();

            _runningCoroutine = null;
            finalAction?.Invoke();
        }
    }
}
