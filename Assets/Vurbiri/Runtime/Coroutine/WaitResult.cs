using System;
using UnityEngine;

namespace Vurbiri
{
    public class WaitResult<T> : CustomYieldInstruction
    {
        private bool _keepWaiting = true;

        public T Result { get; private set; }
        public override bool keepWaiting => _keepWaiting;

        public static WaitResult<T> Empty { get; } = new(default);

        public event Action<T> EventCompleted;

        public WaitResult()
        {
            _keepWaiting = true;
        }
        public WaitResult(T result) => SetResult(result);

        public void SetResult(T result)
        {
            Result = result;
            _keepWaiting = false;

            EventCompleted?.Invoke(result);
        }

        public WaitResult<T> Recreate()
        {
            Result = default;
            _keepWaiting = false;

            return new();
        }

        public void Cancel()
        {
            Result = default;
            _keepWaiting = false;
        }
    }
}
