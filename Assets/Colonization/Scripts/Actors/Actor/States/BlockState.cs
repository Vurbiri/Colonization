//Assets\Colonization\Scripts\Actors\Actor\States\BlockState.cs
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.EffectsFactory;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public class BlockState : AActionState
        {
            private readonly EffectCode _code;
            private readonly int _value;

            public BlockState(int cost, int value, Actor parent) : base(parent, cost)
            {
                _code = new(_actor.TypeId, _actor.Id, BLOCK_SKILL_ID, BLOCK_EFFECT_ID);
                _value = value;
            }

            public override void Enter()
            {
                if (_actor.ContainsEffect(_code))
                    return;

                _skin.Block(true);
                _actor.AddEffect(CreateBlockEffect(_code, _value));
                Pay();
            }

            public override void Exit()
            {
                if (!_actor.ContainsEffect(_code))
                    _skin.Block(false);
            }
        }
	}
}
