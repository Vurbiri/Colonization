using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive.Collections;

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
                _code = new(_actor.TypeId, _actor.Id, 7, 0);
                _value = value;
            }

            public override void Enter()
            {
                _skin.Block();

                ReactiveEffect blockEffect = new(_code, ABILITY, MOD, _value, DURATION);
                blockEffect.Subscribe(OnExit);

                _actor.AddEffect(blockEffect);
                Pay();
            }

            private void OnExit(ReactiveEffect effect, TypeEvent type)
            {
                if (type == TypeEvent.Remove)
                    _fsm.Default();
            }
        }
	}
}
