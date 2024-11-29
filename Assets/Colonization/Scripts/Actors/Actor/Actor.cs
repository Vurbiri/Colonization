//Assets\Colonization\Scripts\Actors\Actor\Actor.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveElementMono<Actor>, ISelectable, IPositionable, IDisposable
    {
        #region Fields
        protected int _typeId;
        protected int _id;
        protected Id<PlayerId> _owner;

        protected AbilitiesSet<ActorAbilityId> _abilities;
        protected Ability<ActorAbilityId> _currentHP;
        protected Ability<ActorAbilityId> _currentAP;
        protected Ability<ActorAbilityId> _move;
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
        protected BlockState _blockState;
        protected TargetState _targetState;
        #endregion

        #region Propirties
        public int TypeId => _typeId;
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public int ActionPoint => _currentAP.Value;
        public bool IsIdle => _stateMachine.IsDefaultState;
        public bool IsBlock => _stateMachine.CurrentState == _blockState;
        public Vector3 Position => _thisTransform.position;
        public AbilitiesSet<ActorAbilityId> Abilities => _abilities;
        #endregion

        #region States
        public bool CanMove() => _move.IsValue;

        public virtual void Move()
        {
            _stateMachine.SetState<MoveState>();
        }

        public virtual void Block()
        {
            _stateMachine.SetState<BlockState>();
        }

        public virtual void UseSkill(int id)
        {
            _stateMachine.SetState<ASkillState>(id);
        }
        #endregion

        public Relation GetRelation(Id<PlayerId> id) => _diplomacy.GetRelation(id, _owner);
        public bool IsCanUseSkill(Id<PlayerId> id, Relation targetAttack, out bool isFriendly)
        {
            return _diplomacy.IsCanActorsInteraction(id, _owner, targetAttack, out isFriendly);
        }

        public void AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(AEffect effect)
        {
            Debug.Log($"currentHP {_currentHP.Value}");
            int delta = _abilities.AddPerk(effect);
            Debug.Log($"currentHP {_currentHP.Value}");

            if(delta != 0)
                actionThisChange?.Invoke(this, TypeEvent.Change);
            return delta;
        }
        public bool ReactionToAttack(bool isTargetReact) => !_targetState.Update(isTargetReact);

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
            array[i++] = new int[] { _id, _currentHP.Value, _currentAP.Value, _move.Value , IsBlock ? 1 : 0 };
            
            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray();

            return array;
        }

        public void Dispose()
        {
            Debug.Log("Dispose");
            _skin.Dispose();
            _stateMachine.Dispose();
            Destroy(gameObject);
        }

        private void BecomeTarget(Id<PlayerId> initiator, Relation relation)
        {
            _stateMachine.SetState(_targetState);
            _diplomacy.ActorsInteraction(_owner, initiator, relation);
        }
        private bool IsCanUseSkill(Id<PlayerId> id, Relation targetAttack) => _diplomacy.IsCanActorsInteraction(id, _owner, targetAttack, out _);

        

        private void RemoveWallDefenceEffect()
        {
            if (_wallDefenceEffect != null)
                _effects.Remove(_wallDefenceEffect);
        }

        private void OnStartTurn(Id<PlayerId> prev, Id<PlayerId> current)
        {
            if (_owner == prev)
            {
                _currentHP.Value += _abilities.GetValue(ActorAbilityId.HPPerTurn);
                _currentAP.Value += _abilities.GetValue(ActorAbilityId.APPerTurn);
                _move.IsValue = true;
                Debug.Log("Выключить Collider");
                return;
            }

            if (_owner == current)
            {
                //Debug.Log("Включить Collider если его ход");
                _effects.Next();
                _stateMachine.ToDefaultState();

                _wallDefenceEffect = EffectsFactory.CreateWallDefenceEffect(_currentHex.GetMaxDefense());
                if (_wallDefenceEffect != null)
                    _effects.Add(_wallDefenceEffect);
            }
        }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type)
        {
            actionThisChange?.Invoke(this, TypeEvent.Change);
        }

        private int CurrentHPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxHP));
        private int CurrentAPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxAP));

    }
}
