//Assets\Colonization\Scripts\Actors\Actor\States\BlockState.cs
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

            public BlockState(int cost, int value, Actor parent) : base(parent, cost, TypeIdKey.Get<BlockState>(0))
            {
                _code = new(_actor.TypeId, _actor.Id, EffectsFactory.BLOCK_SKILL_ID, EffectsFactory.BLOCK_EFFECT_ID);
                _value = value;
                _effects = _actor._effects;
            }

            public override void Enter()
            {
                if (!_effects.Contains(_code))
                {
                    _skin.Block(true);
                    _effects.Add(EffectsFactory.CreateBlockEffect(_code, _value));
                    Pay();
                }

                _actor.ColliderEnable();
            }

            public override void Exit()
            {
                _actor.ColliderDisable();

                if (!_effects.Contains(_code))
                    _skin.Block(false);
            }
        }
    }
}
