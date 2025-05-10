//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\Abstract\ALoadingStep.cs
using System.Collections;
using Vurbiri.EntryPoint;

namespace Vurbiri
{
    public abstract class ALoadingStep : ILoadingStep
    {
        public const float MIN_WEIGHT = ILoadingStep.MIN_WEIGHT;

        private readonly float _weight = MIN_WEIGHT;
        private readonly string _desc;

        public float Weight => _weight;
        public string Description => _desc;

        public ALoadingStep(string desc)
        {
            _desc = desc;
        }

        public ALoadingStep(float weight, string desc)
        {
            _weight = System.Math.Clamp(weight, MIN_WEIGHT, float.MaxValue);
            _desc = desc;
        }

        public abstract IEnumerator GetEnumerator();
    }
}
