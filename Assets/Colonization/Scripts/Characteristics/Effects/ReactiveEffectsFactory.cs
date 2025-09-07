namespace Vurbiri.Colonization.Characteristics
{
    public class ReactiveEffectsFactory
    {
        public static readonly EffectCode WallEffectCode = new(CONST.WALL_TYPE, 0, 0, 0);

        public static ReactiveEffect CreateWallDefenceEffect(int value)
        {
            if (value <= 0) return null;

            return new(WallEffectCode, ActorAbilityId.Defense, TypeModifierId.Addition, value << CONST.WALL_SHIFT, CONST.WALL_DURATION, CONST.WALL_SKIP);
        }
    }
}
