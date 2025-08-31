using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public partial class Warrior
    {
        sealed public partial class WarriorStates
        {
            sealed private class BlockState : AActionState
            {
                private readonly EffectCode _code;
                private readonly int _value;

                public bool IsApplied
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => ActorEffects.Contains(_code);
                }

                public BlockState(int cost, int value, WarriorStates parent) : base(parent, cost)
                {
                    _code = new(parent._actor.TypeId, parent._actor.Id, ReactiveEffectsFactory.BLOCK_SKILL_ID, ReactiveEffectsFactory.BLOCK_EFFECT_ID);
                    _value = value;
                }

                public override void Enter()
                {
                    if (!IsApplied)
                    {
                        Skin.Block(true);
                        ActorEffects.Add(ReactiveEffectsFactory.CreateBlockEffect(_code, _value));
                        Pay();
                    }

                    Actor.Interactable = true;
                }

                public override void Exit()
                {
                    Actor.Interactable = false;

                    if (!IsApplied)
                        Skin.Block(false);
                }

                public override void Select() => GameContainer.TriggerBus.TriggerActorSelect(Actor);
                public override void Unselect(ISelectable newSelectable) => GameContainer.TriggerBus.TriggerUnselect(Actor.Equals(newSelectable));
            }
        }
    }
}
