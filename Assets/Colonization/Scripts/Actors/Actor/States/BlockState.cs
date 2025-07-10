using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        sealed protected class BlockState : AActionState
        {
            private readonly EffectCode _code;
            private readonly EffectsSet _effects;
            private readonly int _value;

            public bool Enabled => _actor._effects.Contains(_code);

            public BlockState(int cost, int value, Actor parent) : base(parent, cost)
            {
                _code = new(parent.TypeId, parent.Id, ReactiveEffectsFactory.BLOCK_SKILL_ID, ReactiveEffectsFactory.BLOCK_EFFECT_ID);
                _value = value;
                _effects = parent._effects;
            }

            public override void Enter()
            {
                if (!_effects.Contains(_code))
                {
                    _skin.Block(true);
                    _effects.Add(ReactiveEffectsFactory.CreateBlockEffect(_code, _value));
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

            public override void Select() => s_triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable)
            {
                s_triggerBus.TriggerUnselect(_actor.Equals(newSelectable));
            }
        }
    }
}
