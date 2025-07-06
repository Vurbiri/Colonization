namespace Vurbiri.Colonization.Characteristics
{
    using static ActorAbilityId;

    public class ReactiveEffectsFactory
    {
        public const int WALL_TYPE = 2, SPELL_TYPE = 3;

        public const int WALL_DURATION = 1, WALL_SKIP = 9, WALL_ADD_SHIFT = 3, WALL_SHIFT = SHIFT_ABILITY + WALL_ADD_SHIFT;
        public const int BLOCK_DURATION = 1, BLOCK_SKIP = 0, BLOCK_SKILL_ID = 7, BLOCK_EFFECT_ID = 0;
        public const int BLESS_SKILL_ID = 0, WRATH_SKILL_ID = 1;

        public static readonly EffectCode WallEffectCode = new(WALL_TYPE, 0, 0, 0);

        public static ReactiveEffect CreateBlockEffect(EffectCode code, int value) => new(code, Defense, TypeModifierId.Addition, value, BLOCK_DURATION, BLOCK_SKIP);

        public static ReactiveEffect CreateWallDefenceEffect(int value)
        {
            if (value <= 0) return null;

            return new(WallEffectCode, Defense, TypeModifierId.Addition, value << WALL_SHIFT, WALL_DURATION, WALL_SKIP);
        }
    }
}
