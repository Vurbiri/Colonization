namespace Vurbiri.Colonization.UI
{
    public class EffectSettingsUI
    {
        public bool isNegative;
        public int value;
        public int duration;
        public string keyDesc;

        public EffectSettingsUI(EffectSettings effect)
        {
            isNegative = effect.Type == EffectTypeId.Negative;
            value = effect.Value;
            duration = effect.Duration;
            keyDesc = effect.KeyDescription;
        }
    }
}
