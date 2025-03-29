//Assets\Colonization\Scripts\Actors\Actor\States\BlockState.cs
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.EffectsFactory;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        protected class BlockState : AActionState
        {
            private readonly EffectCode _code;
            private readonly EffectsSet _effects;
            private readonly int _value;

            public bool Enabled
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _actor._effects.Contains(_code);
            }

            public BlockState(int cost, int value, Actor parent) : base(parent, cost, TypeIdKey.Get<BlockState>(0))
            {
                _code = new(_actor.TypeId, _actor.Id, BLOCK_SKILL_ID, BLOCK_EFFECT_ID);
                _value = value;
                _effects = _actor._effects;
            }

            public static BlockState Create(Id<PlayerId> owner, int cost, int value, Actor parent)
            {
                return owner == PlayerId.Player ? new PlayerBlockState(cost, value, parent) : new BlockState(cost, value, parent);
            }

            public override void Enter()
            {
                if (_effects.Contains(_code))
                    return;

                _skin.Block(true);
                _effects.Add(CreateBlockEffect(_code, _value));
                Pay();
            }

            public override void Exit()
            {
                if (!_effects.Contains(_code))
                    _skin.Block(false);
            }
        }
        //=======================================================================================
        sealed protected class PlayerBlockState : BlockState
        {
            private readonly Collider _actorCollider;

            public PlayerBlockState(int cost, int value, Actor parent) : base(cost, value, parent) => _actorCollider = parent._thisCollider;

            public override void Enter()
            {
                _actorCollider.enabled = _actor._isPlayerTurn;
                base.Enter();
            }

            public override void Exit()
            {
                _actorCollider.enabled = false;
                base.Exit();
            }
        }
    }
}
