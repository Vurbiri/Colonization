namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;
    using System.Collections.Generic;
    using UnityEngine;
    using Vurbiri.Reactive.Collections;

    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveElementMono<Actor>, ISelectable
    {
        #region Fields
        protected int _id;
        protected Id<PlayerId> _owner;
        protected Ability<ActorAbilityId> _currentHP;
        protected Ability<ActorAbilityId> _currentAP;
        protected Ability<ActorAbilityId> _isMove;
        protected Hexagon _currentHex;

        protected AbilitiesSet<ActorAbilityId> _abilities;
        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected GameplayEventBus _eventBus;
        protected float _extentsZ;

        protected readonly ReactiveCollection<Effect> _effects = new();
        protected readonly StateMachineSelectable _stateMachine = new();
        protected List<ASkillState> _skillStates;
        #endregion

        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public int ActionPoint => _currentAP.Value;
        public Vector3 Position => _thisTransform.position;
        public bool CanAction => true;

        public bool CanMove()
        {
            return _isMove.IsBaseValue;
        }
        public void Move()
        {
            _stateMachine.SetState<MoveState>();
        }

        public void Skill(int id)
        {
            _stateMachine.SetState(_skillStates[id]);
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
            int count = _effects.Count + 2;
            int[][] array = new int[count][];
            array[0] = _currentHex.Key.ToArray();
            array[1] = new int[] { _id, _currentHP.Value, _currentAP.Value, _isMove.Value };
            for (int i = 2; i < count; i++)
                array[i] = _effects[i].ToArray();

            return array;
        }

        private void OnEndTurn()
        {
            _currentHP.BaseValue += _abilities.GetValue(ActorAbilityId.HPPerTurn);
            _currentAP.BaseValue += _abilities.GetValue(ActorAbilityId.APPerTurn);
            _isMove.IsBaseValue = true;
        }

        private void OnStartTurn()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
                _effects[i].Next();
        }

        private void AddEffect(EffectSettings effect)
        {
            _effects.Add(effect);
        }



        private void RedirectEvents(Effect item, Operation operation)
        {
            if (operation == Operation.Add)
                _abilities.AddPerk(item);
            else if (operation == Operation.Remove)
                _abilities.RemovePerk(item);

            actionThisChange?.Invoke(this, Operation.Change);
        }

        private int CurrentHPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxHP));
        private int CurrentAPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxAP));
    }
}
