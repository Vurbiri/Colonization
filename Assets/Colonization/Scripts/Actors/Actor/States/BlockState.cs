using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        sealed protected class BlockState : AActionState
        {
            private readonly EffectCode _code;
            private readonly int _value;

            public bool IsApplied
             {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _actor._effects.Contains(_code);
            }

        public BlockState(int cost, int value, Actor parent) : base(parent, cost)
            {
                _code = new(parent.TypeId, parent.Id, ReactiveEffectsFactory.BLOCK_SKILL_ID, ReactiveEffectsFactory.BLOCK_EFFECT_ID);
                _value = value;
            }

            public override void Enter()
            {
                if (!IsApplied)
                    AddEffect();

                ActorInteractable = true;
            }

            public override void Exit()
            {
                ActorInteractable = false;

                if (!IsApplied)
                    _skin.Block(false);
            }

            public override void Select() => GameContainer.TriggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => GameContainer.TriggerBus.TriggerUnselect(_actor.Equals(newSelectable));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void AddEffect()
            {
                _skin.Block(true);
                _actor._effects.Add(ReactiveEffectsFactory.CreateBlockEffect(_code, _value));
                Pay();
            }
        }
    }
}
