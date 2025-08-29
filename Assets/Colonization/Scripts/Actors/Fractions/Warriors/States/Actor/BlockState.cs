using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public partial class Warrior
    {
        sealed private class BlockState : AActionState<WarriorSkin>
        {
            private readonly EffectCode _code;
            private readonly int _value;

            public bool IsApplied
             {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ActorEffects.Contains(_code);
            }

            public BlockState(int cost, int value, Warrior parent) : base(parent, (WarriorSkin)parent._skin, cost)
            {
                _code = new(parent.TypeId, parent.Id, ReactiveEffectsFactory.BLOCK_SKILL_ID, ReactiveEffectsFactory.BLOCK_EFFECT_ID);
                _value = value;
            }

            public override void Enter()
            {
                if (!IsApplied)
                {
                    _skin.Block(true);
                    ActorEffects.Add(ReactiveEffectsFactory.CreateBlockEffect(_code, _value));
                    Pay();
                }

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
        }
    }
}
