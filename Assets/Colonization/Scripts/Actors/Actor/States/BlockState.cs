//Assets\Colonization\Scripts\Actors\Actor\States\BlockState.cs
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.EffectsFactory;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public class BlockState : AState
        {
            public const int DURATION = 1;
            private const int ABILITY = ActorAbilityId.Defense;
            private const int MOD = TypeModifierId.Addition;

            private readonly EffectCode _code;
            private readonly int _value;

            public BlockState(int cost, int value, Actor parent) : base(parent, cost)
            {
                _code = new(_actor.TypeId, _actor.Id, BLOCK_SKILL_ID, BLOCK_EFFECT_ID);
                _value = value;
            }

            public override void Enter()
            {
                _skin.Block();
                _actor.AddEffect(CreateBlockEffect(_code, _value, _fsm.ToDefault));
                Pay();
            }

        }
	}
}
