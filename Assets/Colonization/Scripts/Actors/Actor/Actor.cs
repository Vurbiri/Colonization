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
    public abstract partial class Actor : AReactiveElementMono<Actor>, ISelectable, ICancel, IPositionable, IDisposable
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

        protected ReactiveValue<bool> _isCancel = new(false);

        protected Coroutine _onHitCoroutine, _deathCoroutine;
        #endregion

        #region Propirties
        public int TypeId => _typeId;
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public bool IsPlayer => _owner == PlayerId.Player;
        public int ActionPoint => _currentAP.Value;
        public bool IsIdle => _stateMachine.IsDefaultState;
        public bool IsBlock => _blockState.Enabled;
        public Vector3 Position => _thisTransform.position;
        public ActorSkin Skin => _skin;
        public IReactiveCollection<ReactiveEffect> Effects => _effects;
        public AbilitiesSet<ActorAbilityId> Abilities => _abilities;
        public IReadOnlyReactive<bool> IsCancel => _isCancel;
        #endregion

        #region States
        public bool CanMove() => _move.IsValue;

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

        public void AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(IPerk effect)
        {
            int delta = _abilities.AddPerk(effect);

            if(delta != 0)
                actionThisChange?.Invoke(this, TypeEvent.Change);
            return delta;
        }

        public void ColliderEnable(bool enabled) => _thisCollider.enabled = enabled;

        public virtual void Select()
        {
            _stateMachine.Select();
        }
        public virtual void Unselect(ISelectable newSelectable)
        {
            _stateMachine.Unselect(newSelectable);
        }

        public int[][] ToArray()
        {
            int i = 0;
            int count = _effects.Count;
            int[][] array = new int[count + 2][];

            array[i++] = _currentHex.Key.ToArray();
            array[i++] = new int[] { _id, _currentHP.Value, _currentAP.Value, _move.Value };
            
            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray();

            return array;
        }

        public override bool Equals(Actor other)
        {
            if (other == null) return false;

            return _typeId == other._typeId & _id == other._id & _owner == other._owner;
        }

        public void Dispose()
        {
            _currentHex.ExitActor();
            _skin.Dispose();
            _stateMachine.Dispose();
            Destroy(gameObject);
        }

        private void SkillUsedStart(Id<PlayerId> initiator, Relation relation)
        {
            _stateMachine.SetState<TargetState>();
            _diplomacy.ActorsInteraction(_owner, initiator, relation);
        }

        private void SkillUsedEnd()
        {
            if (_deathCoroutine == null)
            {
                _stateMachine.ToPrevState();
                actionThisChange?.Invoke(this, TypeEvent.Change);
            }
        }

        private bool Hit(bool isTargetReact)
        {
            if (_onHitCoroutine != null)
                StopCoroutine(_onHitCoroutine);

            if (_currentHP.Value <= 0)
            {
                _deathCoroutine = StartCoroutine(Death_Coroutine());
                return true;
            }

            if (isTargetReact)
                _onHitCoroutine = StartCoroutine(Hit_Coroutine());

            return false;
        }

        private IEnumerator Death_Coroutine()
        {
            Removing();
            yield return _skin.Death();
            Dispose();
        }

        private IEnumerator Hit_Coroutine()
        {
            yield return _skin.React();
            _onHitCoroutine = null;
        }

        private void RemoveWallDefenceEffect()
        {
            if (_wallDefenceEffect != null)
            {
                _effects.Remove(_wallDefenceEffect);
                _wallDefenceEffect = null;
            }
        }

        private void OnStartTurn(Id<PlayerId> prev, Id<PlayerId> current)
        {
            if (_owner == prev)
            {
                _currentHP.Next();
                _currentAP.Next();
                _move.On();

                Debug.Log("Защита от стен - решить проблему");
                _wallDefenceEffect = EffectsFactory.CreateWallDefenceEffect(_currentHex.GetMaxDefense());
                if (_wallDefenceEffect != null)
                    _effects.Add(_wallDefenceEffect);

                Debug.Log("Выключить Collider");
                return;
            }

            if (_owner == current)
            {
                //Debug.Log("Включить Collider если его ход");
                _effects.Next();
                _stateMachine.ToDefaultState();
            }
        }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type)
        {
            actionThisChange?.Invoke(this, TypeEvent.Change);
        }
        private void TriggerChange() => actionThisChange?.Invoke(this, TypeEvent.Change);
    }
}
