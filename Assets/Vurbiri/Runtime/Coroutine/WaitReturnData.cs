//Assets\Vurbiri\Runtime\Coroutine\WaitReturnData.cs
using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    public class WaitReturnData<T> : CustomYieldInstruction
    {
        private readonly MonoBehaviour _monoBehaviour;

        public T Return { get; private set; }

        public override bool keepWaiting => _keepWaiting;
        private bool _keepWaiting = true;

        public WaitReturnData(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public WaitReturnData(MonoBehaviour monoBehaviour, Func<Action<T>, IEnumerator> coroutine)
        {
            _monoBehaviour = monoBehaviour;
            Start(coroutine);
        }

        public WaitReturnData<T> Start(Func<Action<T>, IEnumerator> coroutine)
        {
            _keepWaiting = true;
            _monoBehaviour.StartCoroutine(coroutine(Callback));
            return this;
        }

        public WaitReturnData<T> Start<U>(Func<U, Action<T>, IEnumerator> coroutine, U value)
        {
            _keepWaiting = true;
            _monoBehaviour.StartCoroutine(coroutine(value, Callback));
            return this;
        }

        public WaitReturnData<T> Start<U, V>(Func<U, V, Action<T>, IEnumerator> coroutine, U value1, V value2)
        {
            _keepWaiting = true;
            _monoBehaviour.StartCoroutine(coroutine(value1, value2, Callback));
            return this;
        }

        private void Callback(T date)
        {
            Return = date;
            _keepWaiting = false;
        }
    }
}


