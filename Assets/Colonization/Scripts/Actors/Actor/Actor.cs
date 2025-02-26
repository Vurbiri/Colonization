//Assets\Colonization\Scripts\Actors\Actor\Actor.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveItemMono<Actor>, ISelectable, ICancel, IPositionable, IDisposable, IJaggedArrayable
    {
        #region Fields
        protected int _typeId;
        protected int _id;
        protected Id<PlayerId> _owner;

        protected AbilitiesSet<ActorAbilityId> _abilities;
        protected SubAbility<ActorAbilityId> _currentHP;
        protected SubAbility<ActorAbilityId> _currentAP;
        protected BooleanAbility<ActorAbilityId> _move;
        protected Hexagon _currentHex;

        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected Collider _thisCollider;
        protected Diplomacy _diplomacy;
        protected GameplayEventBus _eventBus;
        protected float _extentsZ;

        protected EffectsSet _effects;
        protected ReactiveEffect _wallDefenceEffect;

        protected StateMachineSelectable _stateMachine;
        protected ABlockState _blockState;

        protected bool _isPlayerTurn;

        protected ReactiveValue<bool> _canCancel = new(false);

        protected Coroutine _deathCoroutine;
        #endregion

        #region Propirties
        public int TypeId => _typeId;
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public bool IsPlayer => _owner == PlayerId.Player;
        public int ActionPoint => _currentAP.Value;
        public bool CanMove => _move.IsValue;
        public bool IsIdle => _stateMachine.IsDefaultState;
        public bool IsBlock => _blockState.Enabled;
        public bool IsDead => _currentHP.Value <= 0;
        public Vector3 Position => _thisTransform.position;
        public ActorSkin Skin => _skin;
        public IListReactiveItems<ReactiveEffect> Effects => _effects;
        public IReadOnlyAbilities<ActorAbilityId> Abilities => _abilities;
        public IReadOnlyReactive<bool> CanCancel => _canCancel;
        #endregion

        #region States
        public virtual void Move() => _stateMachine.SetState<MoveState>();
        public virtual void Block() => _stateMachine.SetState(_blockState);
        public virtual void UseSkill(int id) => _stateMachine.SetState<ASkillState>(id);
        public virtual void Cancel() => _stateMachine.Cancel();
        #endregion

        public Relation GetRelation(Id<PlayerId> id) => _diplomacy.GetRelation(id, _owner);
        public bool IsCanUseSkill(Id<PlayerId> id, Relation typeAction, out bool isFriendly)
        {
            if(!(_stateMachine.IsDefaultState || _blockState.Enabled))
            {
                isFriendly = false;
                return false;
            }
            
            return _diplomacy.IsCanActorsInteraction(id, _owner, typeAction, out isFriendly);
        }

        public int AddEffect(ReactiveEffect effect) => _effects.AddEffect(effect);
        public int ApplyEffect(IPerk effect)
        {
            int delta = _abilities.AddPerk(effect);

            if(delta != 0)
                _subscriber.Invoke(this, TypeEvent.Change);
            return delta;
        }

        public void ColliderEnable(bool enabled) => _thisCollider.enabled = enabled;
        public void EnablePlayerCollider() => _thisCollider.enabled = _isPlayerTurn;

        #region ISelectable
        public virtual void Select()
        {
            _stateMachine.Select();
        }
        public virtual void Unselect(ISelectable newSelectable)
        {
            _stateMachine.Unselect(newSelectable);
        }
        #endregion

        #region IJaggedArrayable
        private const int ADD_SIZE_ARRAY = 2;
        public int[][] ToArray()
        {
            int i = 0;
            int count = _effects.Count;
            int[][] array = new int[count + ADD_SIZE_ARRAY][];

            array[i++] = _currentHex.Key.ToArray();
            array[i++] = ToSubArray();
            
            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray();

            return array;
        }
        public int[][] ToArray(int[][] array)
        {
            int count = _effects.Count;
            if (array == null || array.Length != count + ADD_SIZE_ARRAY)
                return ToArray();

            int i = 0;
            array[i] = _currentHex.Key.ToArray(array[i++]);
            array[i] = ToSubArray(array[i++]);

            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray(array[i]);

            return array;
        }
        private const int SIZE_SUBARRAY = 4;
        private int[] ToSubArray() => new int[] { _id, _currentHP.Value, _currentAP.Value, _move.Value };
        private int[] ToSubArray(int[] array)
        {
            if (array == null || array.Length != SIZE_SUBARRAY)
                return ToSubArray();

            int i = 0;
            array[i++] = _id; array[i++] = _currentHP.Value; array[i++] = _currentAP.Value; array[i] = _move.Value;
            return array;
        }
        #endregion

        public override bool Equals(Actor other) => System.Object.ReferenceEquals(this, other);

        public void Dispose()
        {
            _currentHex.ExitActor();
            _skin.Dispose();
            _stateMachine.Dispose();
            Destroy(gameObject);
        }

        #region BecomeTarget
        private void BecomeTargetStart(Id<PlayerId> initiator, Relation relation)
        {
            _stateMachine.SetState<TargetState>();
            _diplomacy.ActorsInteraction(_owner, initiator, relation);
        }

        private void BecomeTargetEnd()
        {
            if (_deathCoroutine == null)
            {
                _stateMachine.ToPrevState();
                _subscriber.Invoke(this, TypeEvent.Change);
            }
        }
        #endregion

        private IEnumerator Death_Cn()
        {
            Removing();
            yield return _skin.Death();
            Dispose();
        }

        #region WallDefence
        private void AddWallDefenceEffect()
        {
            _wallDefenceEffect = EffectsFactory.CreateWallDefenceEffect(_currentHex.GetMaxDefense());
            if (_wallDefenceEffect != null)
                _effects.AddEffect(_wallDefenceEffect);
        }
        private void RemoveWallDefenceEffect()
        {
            if (_wallDefenceEffect != null)
            {
                _effects.Remove(_wallDefenceEffect);
                _wallDefenceEffect = null;
            }
        }
        #endregion

        private void OnNextTurn(ITurn turn)
        {
            if (_owner == turn.PreviousId)
            {
                _currentHP.Next();
                _currentAP.Next();
                _move.On();

                AddWallDefenceEffect();

                _isPlayerTurn = _thisCollider.enabled = false;
                return;
            }

            if (_owner == turn.CurrentId)
            {
                _isPlayerTurn = _thisCollider.enabled = _owner == PlayerId.Player;

                _effects.Next();
                _stateMachine.ToDefaultState();
            }
        }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type)
        {
            _subscriber.Invoke(this, TypeEvent.Change);
        }
        private void TriggerChange() => _subscriber.Invoke(this, TypeEvent.Change);
    }
}
