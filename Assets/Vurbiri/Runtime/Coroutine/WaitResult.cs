//Assets\Vurbiri\Runtime\Coroutine\WaitResult.cs
using UnityEngine;

namespace Vurbiri
{
    sealed public class WaitResult<T> : CustomYieldInstruction
    {
        private bool _keepWaiting = true;
        private T _result;

        public T Result => _result;
        public override bool keepWaiting => _keepWaiting;

        public static WaitResult<T> Empty { get; } = new(default);

        public WaitResult()
        {
            _keepWaiting = true;
        }
        public WaitResult(T result) => SetResult(result);

        public WaitResult<T> SetResult(T result)
        {
            _result = result;
            _keepWaiting = false;

            return this;
        }

        public WaitResult<T> Recreate()
        {
            _result = default;
            _keepWaiting = false;

            return new();
        }

        public WaitResult<T> Cancel()
        {
            _result = default;
            _keepWaiting = false;

            return this;
        }

        public override void Reset() => _keepWaiting = true;
    }
}
