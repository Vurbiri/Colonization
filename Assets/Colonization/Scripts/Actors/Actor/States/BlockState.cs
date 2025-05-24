using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        sealed protected class BlockState : AActionState
        {
            private readonly EffectCode _code;
            private readonly EffectsSet _effects;
            private readonly GameplayTriggerBus _triggerBus;
            private readonly int _value;

            public bool Enabled => _actor._effects.Contains(_code);

            public BlockState(int cost, int value, Actor parent) : base(parent, cost, TypeIdKey.Get<BlockState>(0))
            {
                _code = new(parent.TypeId, parent.Id, EffectsFactory.BLOCK_SKILL_ID, EffectsFactory.BLOCK_EFFECT_ID);
                _value = value;
                _effects = parent._effects;
                _triggerBus = parent._triggerBus;
            }

            public override void Enter()
            {
                Debug.Log($"Enter BlockState {_effects.Contains(_code)} {_skin.name}");
                if (!_effects.Contains(_code))
                {
                    _skin.Block(true);
                    _effects.Add(EffectsFactory.CreateBlockEffect(_code, _value));
                    Pay();
                }

                _actor.Interactable = true;
            }

            public override void Exit()
            {
                _actor.Interactable = false;

                if (!_effects.Contains(_code))
                    _skin.Block(false);
            }

            public override void Select() => _triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable)
            {
                _triggerBus.TriggerUnselect(_actor.Equals(newSelectable));
            }
        }
    }
}
