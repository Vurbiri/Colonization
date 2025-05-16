//Assets\Colonization\Scripts\EntryPoint\Abstract\ALocalizationLoadingStep.cs
using System.Collections;
using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
    public abstract class ALocalizationLoadingStep : ILoadingStep
    {
        private readonly float _weight = ILoadingStep.MIN_WEIGHT;
        private readonly string _key;

        public float Weight => _weight;
        public string Description => Localization.Instance.GetText(Files.Main, _key);

        protected ALocalizationLoadingStep(string key)
        {
            _key = key;
        }

        public ALocalizationLoadingStep(float weight, string key)
        {
            _weight = System.Math.Clamp(weight, ILoadingStep.MIN_WEIGHT, float.MaxValue);
            _key = key;
        }

        public abstract IEnumerator GetEnumerator();
    }
}
