using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public class WaitQueue : CustomYieldInstruction
    {
        private readonly Queue<IEnumerator> _coroutines = new();
        private readonly MonoBehaviour _monoBehaviour;
        private IEnumerator _currentCoroutine = null;
        private readonly Action finalAction;

        public override bool keepWaiting => _currentCoroutine != null;

        public WaitQueue(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }
        public WaitQueue(MonoBehaviour monoBehaviour, Action finalAction)
        {
            _monoBehaviour = monoBehaviour;
            this.finalAction = finalAction;
        }
        public WaitQueue(MonoBehaviour monoBehaviour, IEnumerator coroutine)
        {
            _monoBehaviour = monoBehaviour;
            _monoBehaviour.StartCoroutine(AddCoroutine(coroutine));
        }

        public void StopAndClear(bool isRunFinalAction)
        {
            if (_currentCoroutine != null)
            {
                _monoBehaviour.StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }

            _coroutines.Clear();

            if(isRunFinalAction)
                finalAction?.Invoke();
        }

        public WaitQueue Add(IEnumerator coroutine)
        {
            _monoBehaviour.StartCoroutine(AddCoroutine(coroutine));
            return this;
        }

        private IEnumerator AddCoroutine(IEnumerator coroutine)
        {
            _coroutines.Enqueue(coroutine);

            if (_currentCoroutine != null)
                yield break;

            while (_coroutines.Count > 0)
                yield return _currentCoroutine = _coroutines.Dequeue();

            _currentCoroutine = null;
            finalAction?.Invoke();
        }
    }
}
