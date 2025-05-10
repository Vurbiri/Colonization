//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\Abstract\AEnumeratorStep.cs
using System.Collections;

namespace Vurbiri.EntryPoint
{
    public abstract class AEnumeratorStep : ALoadingStep, IEnumerator
    {
        protected AEnumeratorStep(string desc) : base(desc) { }
        protected AEnumeratorStep(float weight, string desc) : base(weight, desc) { }

        public object Current => null;

        public abstract bool MoveNext();

        sealed public override IEnumerator GetEnumerator() => this;

        public void Reset() { }
    }
}
