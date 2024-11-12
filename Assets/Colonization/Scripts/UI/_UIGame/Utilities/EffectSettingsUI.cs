namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    public class EffectSettingsUI
    {
        public bool isNegative;
        public int value;
        public int duration;
        public bool isInstant;
        public string keyDesc;

        public EffectSettingsUI(EffectSettings effect)
        {
            isNegative = effect.TargetActor == TargetOfEffectId.Enemy;
            value = effect.Value;
            duration = effect.Duration;
            keyDesc = KEYS_DESK_EFFECTS[effect.KeyDescId];

            isInstant = duration == 0;
            if (effect.TypeOperation != TypeOperationId.Percent & effect.TargetAbility <= ActorAbilityId.MAC_RATE_ABILITY)
                value /= ActorAbilityId.RATE_ABILITY;
        }
    }
}
