//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\Interface\ILoadingStep.cs
using System.Collections;

namespace Vurbiri.EntryPoint
{
    public interface ILoadingStep : IEnumerable
    {
        protected const float MIN_WEIGHT = 0.1f;

        public float Weight { get; }
        public string Description { get; }
    }
}
