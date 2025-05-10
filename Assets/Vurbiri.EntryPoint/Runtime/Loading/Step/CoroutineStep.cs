//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\CoroutineStep.cs
using System.Collections;

namespace Vurbiri.EntryPoint
{
    sealed public class CoroutineStep : ALoadingStep
    {
        private readonly IEnumerator _coroutine;

        public CoroutineStep(IEnumerator coroutine, string desc, float weight) : base(weight, desc)
        {
            _coroutine = coroutine;
        }
        public CoroutineStep(IEnumerator coroutine, string desc) : this(coroutine, desc, MIN_WEIGHT) { }
        public CoroutineStep(IEnumerator coroutine) : this(coroutine, string.Empty, MIN_WEIGHT) { }
        public CoroutineStep(IEnumerator coroutine, float weight) : this(coroutine, string.Empty, weight) { }

        public override IEnumerator GetEnumerator() => _coroutine;
    }
}
