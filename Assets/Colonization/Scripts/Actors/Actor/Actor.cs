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

        protected readonly StateMachineSelectable _stateMachine = new(new TargetState());
        #endregion

        #region Propirties
        public int TypeId => _typeId;
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public int ActionPoint => _currentAP.Value;
        public Vector3 Position => _thisTransform.position;
        public AbilitiesSet<ActorAbilityId> Abilities => _abilities;
        #endregion

        #region States
        public bool CanMove() => _move.IsValue;

        public void Move()
        {
            _stateMachine.SetState<MoveState>();
        }

        public void Block()
        {
            _stateMachine.SetState<BlockState>();
        }

        public void UseSkill(int id)
        {
            _stateMachine.SetState<ASkillState>(id);
        }
        #endregion

        public Relation GetRelation(Id<PlayerId> id) => _diplomacy.GetRelation(id, _owner);

        public void AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(AEffect effect)
        {
            int delta = _abilities.AddPerk(effect);
            actionThisChange?.Invoke(this, TypeEvent.Change);
            return delta;
        }

        public void EndTurn()
        {
            _currentHP.Value += _abilities.GetValue(ActorAbilityId.HPPerTurn);
            _currentAP.Value += _abilities.GetValue(ActorAbilityId.APPerTurn);
            _move.IsValue = true;
            Debug.Log("Выключить Collider");
            //if(_stateMachine.CurrentState != _blockState)
            //    _stateMachine.ToDefault();
        }
        public void StartTurn()
        {
            Debug.Log("Включить Collider если игрок и его ход");
            _effects.Next();
            _stateMachine.ToDefaultState();

            _wallDefenceEffect = EffectsFactory.CreateWallDefenceEffect(_currentHex.GetDefense());
            if (_wallDefenceEffect != null)
                _effects.Add(_wallDefenceEffect);
        }

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
            bool isBlock = _stateMachine.IsCurrentState<BlockState>();
            int[][] array = new int[count + 2][];

            array[i++] = _currentHex.Key.ToArray();
            array[i++] = new int[] { _id, _currentHP.Value, _currentAP.Value, _move.Value , isBlock ? 1 : 0 };
            
            for (int j = 0; j < count; j++, i++)
                array[i] = _effects[j].ToArray();

            return array;
        }

        public void Dispose()
        {
            _skin.Dispose();
            _stateMachine.Dispose();
        }

        private void BecomeTarget(Id<PlayerId> initiator, Relation relation)
        {
            _stateMachine.SetState<TargetState>();

            if (initiator == _owner)
                return;
        }

        private bool ReactionToAttack(bool _isTargetReact)
        {
            if (_currentHP.Value <= 0)
            {
                _skin.Death();
                return true;
            }

            if (_isTargetReact)
                _skin.React();

            return false;

        }

        private void RemoveWallDefenceEffect()
        {
            if (_wallDefenceEffect != null)
                _effects.Remove(_wallDefenceEffect);
        }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type)
        {
            actionThisChange?.Invoke(this, TypeEvent.Change);
        }

        private int CurrentHPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxHP));
        private int CurrentAPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxAP));

    }
}
