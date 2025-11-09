using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class WallEffectFactory
    {
        public static readonly EffectCode WallEffectCode = new(WALL_TYPE, 0, 0, 0);

        public static ReactiveEffect Create(int value)
        {
            return value > 0 ? new(WallEffectCode, ActorAbilityId.Defense, TypeModifierId.Addition, value << WALL_SHIFT, WALL_DURATION, WALL_SKIP) : null;
        }
    }
}
