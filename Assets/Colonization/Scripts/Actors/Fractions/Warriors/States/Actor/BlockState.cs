using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;

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

                public BlockState(WarriorStates parent, SpecSkillSettings specSkill) : base(parent, specSkill.Cost)
                {
                    _code = new(parent._actor.TypeId, parent._actor.Id, SPEC_SKILL_ID, 0);
                    _value = specSkill.Value;
                }

                public override void Enter()
                {
                    if (!IsApplied)
                    {
                        Skin.Block(true);
                        ActorEffects.Add(new(_code, ActorAbilityId.Defense, TypeModifierId.Addition, _value, BLOCK_DURATION, BLOCK_SKIP));
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
