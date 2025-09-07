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
                private readonly EffectCode _effectCode;
                private readonly int _value;

                public bool IsApplied 
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => ActorEffects.Contains(_effectCode); 
                }

                public BlockState(SpecSkillSettings specSkill, WarriorStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _effectCode = code;
                    _value = specSkill.Value;
                }

                public override void Enter()
                {
                    if (!IsApplied)
                    {
                        Skin.Block(true);
                        ActorEffects.Add(new(_effectCode, ActorAbilityId.Defense, TypeModifierId.Addition, _value, CONST.BLOCK_DURATION, CONST.BLOCK_SKIP));
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
