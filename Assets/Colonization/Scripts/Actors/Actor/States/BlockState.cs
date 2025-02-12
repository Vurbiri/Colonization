//Assets\Colonization\Scripts\Actors\Actor\States\BlockState.cs
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.EffectsFactory;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public abstract class ABlockState : AActionState
        {
            private readonly EffectCode _code;
            private readonly EffectsSet _effects;
            private readonly int _value;

            public bool Enabled => _effects.Contains(_code);

            public ABlockState(int cost, int value, Actor parent) : base(parent, cost, TypeIdKey.Get<ABlockState>(0))
            {
                _code = new(_actor.TypeId, _actor.Id, BLOCK_SKILL_ID, BLOCK_EFFECT_ID);
                _value = value;
                _effects = _actor._effects;
            }

            public static ABlockState Create(int cost, int value, Actor parent)
            {
                UnityEngine.Debug.Log("разкомментить PlayerBlockState/AIBlockState ");
                //return parent._owner == PlayerId.Player ? new PlayerBlockState(cost, value, parent) : new AIBlockState(cost, value, parent);
                return new PlayerBlockState(cost, value, parent);
            }

            public override void Enter()
            {
                if (_effects.Contains(_code))
                    return;

                _skin.Block(true);
                _effects.AddEffect(CreateBlockEffect(_code, _value));
                Pay();
            }

            public override void Exit()
            {
                if (!_effects.Contains(_code))
                    _skin.Block(false);
            }
        }

        public class AIBlockState : ABlockState
        {
            public AIBlockState(int cost, int value, Actor parent) : base(cost, value, parent) { }
        }

        public class PlayerBlockState : ABlockState
        {
            public PlayerBlockState(int cost, int value, Actor parent) : base(cost, value, parent) { }

            public override void Enter()
            {
                _actor.ColliderEnable(true);
                base.Enter();
            }

            public override void Exit()
            {
                _actor.ColliderEnable(false);
                base.Exit();
            }
        }
    }
}
