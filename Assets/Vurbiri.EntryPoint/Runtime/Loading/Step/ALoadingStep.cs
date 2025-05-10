//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\ALoadingStep.cs
using System.Collections;

namespace Vurbiri
{
    public abstract class ALoadingStep : IEnumerator
    {
        public const float DEFAULT_WEIGHT = 0.1f;

        protected string _desc;

        public virtual float Weight => DEFAULT_WEIGHT;
        public string Description => _desc;
        public object Current => null;

        public ALoadingStep(string desc)
        {
            _desc = desc;
        }

        public abstract bool MoveNext();

        public void Reset() { }
    }
}
