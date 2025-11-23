using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public class CoroutinesQueue : Enumerator, System.IDisposable
    {
        private readonly Queue<IEnumerator> _coroutines = new();
        private readonly Action _finalAction;
        private MonoBehaviour _mono;
        private Coroutine _runningCoroutine = null;

        public int Count { [Impl(256)] get => _coroutines.Count; }
        public bool IsRunning { [Impl(256)] get => _runningCoroutine != null; }

        [Impl(256)] public CoroutinesQueue() => _mono = CoroutineInternal.Instance;
        [Impl(256)] public CoroutinesQueue(MonoBehaviour mono) => _mono = mono;
        [Impl(256)] public CoroutinesQueue(Action finalAction) : this(finalAction, CoroutineInternal.Instance) { }
        [Impl(256)] public CoroutinesQueue(Action finalAction, MonoBehaviour mono)
        {
            _mono = mono;
            _finalAction = finalAction;
        }
        [Impl(256)] public CoroutinesQueue(IEnumerator coroutine) : this(coroutine, CoroutineInternal.Instance) { }
        [Impl(256)] public CoroutinesQueue(IEnumerator coroutine, MonoBehaviour mono)
        {
            _mono = mono;
            Enqueue(coroutine);
        }
        [Impl(256)] public CoroutinesQueue(Action finalAction, IEnumerator coroutine) : this(finalAction, coroutine, CoroutineInternal.Instance) { }
        [Impl(256)] public CoroutinesQueue(Action finalAction, IEnumerator coroutine, MonoBehaviour monoBehaviour)
        {
            _mono = monoBehaviour;
            _finalAction = finalAction;
            Enqueue(coroutine);
        }

        public void Enqueue(IEnumerator coroutine)
        {
            _coroutines.Enqueue(coroutine);
            _runningCoroutine ??= _mono.StartCoroutine(Run_Cn());
        }

        public void StopAndClear(bool runFinalAction)
        {
            if (_runningCoroutine != null)
            {
                _mono.StopCoroutine(_runningCoroutine);
                _runningCoroutine = null;
            }

            _coroutines.Clear();

            if(runFinalAction)
                _finalAction?.Invoke();
        }

        public override bool MoveNext() => _runningCoroutine != null;

        public void Dispose()
        {
            if (_runningCoroutine != null)
                _mono.StopCoroutine(_runningCoroutine);
        }

        private IEnumerator Run_Cn()
        {
            while (_coroutines.Count > 0)
                yield return _coroutines.Dequeue();

            _runningCoroutine = null;
            _finalAction?.Invoke();
        }
    }
}
