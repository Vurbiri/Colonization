//Assets\Colonization\Scripts\Characteristics\Effects\EffectsFactory.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class EffectsFactory
    {
        private const int ABILITY = ActorAbilityId.Defense;
        private const int MOD = TypeModifierId.Addition;
        private const int DURATION = 1;
        private static readonly EffectCode WallEffectCode = new(3, 6, 6, 6);

        public const int BLOCK_DURATION = DURATION, BLOCK_SKILL_ID = 7, BLOCK_EFFECT_ID = 0;

        public static ReactiveEffect CreateBlockEffect(EffectCode code, int value) => new(code, ABILITY, MOD, value, BLOCK_DURATION);

        public static ReactiveEffect CreateWallDefenceEffect(int value)
        {
            if(value <= 0) 
                return null;

            return new(WallEffectCode, ABILITY, MOD, value, DURATION);
        }
    }
}
