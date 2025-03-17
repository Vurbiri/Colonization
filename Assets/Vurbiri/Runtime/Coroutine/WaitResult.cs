//Assets\Vurbiri\Runtime\Coroutine\WaitResult.cs
using UnityEngine;

namespace Vurbiri
{
    public abstract class WaitResult<T> : CustomYieldInstruction
    {
        protected bool _keepWaiting = true;
        protected T _value;

        public T Value => _value;
        sealed public override bool keepWaiting => _keepWaiting;

        sealed public override void Reset() => _keepWaiting = true;
    }

    public class WaitResultSource<T> : WaitResult<T>
    {
        public static WaitResultSource<T> Empty { get; } = new(default);

        public WaitResultSource()
        {
            _keepWaiting = true;
        }
        public WaitResultSource(T result)
        {
            _value = result;
            _keepWaiting = false;
        }

        public WaitResult<T> SetResult(T result)
        {
            _value = result;
            _keepWaiting = false;

            return this;
        }

        public WaitResultSource<T> Recreate()
        {
            _value = default;
            _keepWaiting = false;

            return new();
        }

        public WaitResult<T> Cancel()
        {
            _value = default;
            _keepWaiting = false;

            return this;
        }
    }
}
