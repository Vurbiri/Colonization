using System.Collections;

namespace Vurbiri.EntryPoint
{
    public abstract class ALoadingStep : ILoadingStep
    {
        private readonly float _weight = ILoadingStep.MIN_WEIGHT;
        private readonly string _desc;

        public float Weight => _weight;
        public string Description => _desc;

        public ALoadingStep(string desc)
        {
            _desc = desc;
        }

        public ALoadingStep(float weight, string desc)
        {
            _weight = System.Math.Clamp(weight, ILoadingStep.MIN_WEIGHT, float.MaxValue);
            _desc = desc;
        }

        public abstract IEnumerator GetEnumerator();
    }
}
