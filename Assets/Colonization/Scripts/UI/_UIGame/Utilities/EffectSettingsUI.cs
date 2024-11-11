namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    public class EffectSettingsUI
    {
        public bool isNegative;
        public int value;
        public int duration;
        public string keyDesc;

        public EffectSettingsUI(EffectSettings effect)
        {
            isNegative = effect.TargetActor == TargetOfEffectId.Enemy;
            value = effect.Value;
            duration = effect.Duration;
            keyDesc = KEYS_DESK_EFFECTS[effect.TargetAbility][effect.TypeOperation.Value];
        }
    }
}
