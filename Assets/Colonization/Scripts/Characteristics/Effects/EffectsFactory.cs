//Assets\Colonization\Scripts\Characteristics\Effects\EffectsFactory.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class EffectsFactory
    {
        private const int ABILITY = ActorAbilityId.Defense;
        private const int MOD = TypeModifierId.Addition;
        private const int WALL_RATE = ActorAbilityId.RATE_ABILITY << 3, WALL_DURATION = 3;

        public const int BLOCK_DURATION = 1, BLOCK_SKILL_ID = 7, BLOCK_EFFECT_ID = 0;
        public static readonly EffectCode WallEffectCode = new(3, 0, 0, 0);

        public static ReactiveEffect CreateBlockEffect(EffectCode code, int value) => new(code, ABILITY, MOD, value, BLOCK_DURATION);

        public static ReactiveEffect CreateWallDefenceEffect(int value)
        {
            if(value <= 0) return null;

            return new(WallEffectCode, ABILITY, MOD, value * WALL_RATE, WALL_DURATION);
        }
    }
}
