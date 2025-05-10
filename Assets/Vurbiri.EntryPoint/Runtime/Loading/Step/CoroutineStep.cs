//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\CoroutineStep.cs
using System.Collections;

namespace Vurbiri.EntryPoint
{
    sealed public class CoroutineStep : ALoadingStep
    {
        private readonly IEnumerator _coroutine;
        private readonly float _weight;

        public override float Weight => _weight;

        public CoroutineStep(IEnumerator coroutine, string desc, float weight) : base(desc)
        {
            _coroutine = coroutine;
            _weight = weight;
        }
        public CoroutineStep(IEnumerator coroutine, string desc) : this(coroutine, desc, DEFAULT_WEIGHT) { }
        public CoroutineStep(IEnumerator coroutine) : this(coroutine, string.Empty, DEFAULT_WEIGHT) { }
        public CoroutineStep(IEnumerator coroutine, float weight) : this(coroutine, string.Empty, weight) { }

        public override bool MoveNext() => _coroutine.MoveNext();
    }
}
