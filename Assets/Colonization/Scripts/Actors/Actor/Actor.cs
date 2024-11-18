using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveElementMono<Actor>, ISelectable
    {
        #region Fields
        protected int _id;
        protected Id<PlayerId> _owner;
        protected Ability<ActorAbilityId> _currentHP;
        protected Ability<ActorAbilityId> _currentAP;
        protected Ability<ActorAbilityId> _move;
        protected Hexagon _currentHex;

        protected AbilitiesSet<ActorAbilityId> _abilities;
        protected EffectsSet _effects;
        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected GameplayEventBus _eventBus;
        protected float _extentsZ;

        protected readonly StateMachineSelectable _stateMachine = new();
        protected List<ASkillState> _skillStates;
        #endregion

        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public int ActionPoint => _currentAP.Value;
        public Vector3 Position => _thisTransform.position;
        public AbilitiesSet<ActorAbilityId> Abilities => _abilities;
        public bool CanAction => true;

        public bool CanMove() => _move.IsValue;

        public void Move()
        {
            _stateMachine.SetState<MoveState>();
        }

        public void UseSkill(int id)
        {
            _stateMachine.SetState(_skillStates[id]);
        }


        public int AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(AEffect effect) => _abilities.AddPerk(effect);

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
            array[1] = new int[] { _id, _currentHP.Value, _currentAP.Value, _move.Value };
            for (int i = 2; i < count; i++)
                array[i] = _effects[i].ToArray();

            return array;
        }

        private void OnEndTurn()
        {
            _currentHP.Value += _abilities.GetValue(ActorAbilityId.HPPerTurn);
            _currentAP.Value += _abilities.GetValue(ActorAbilityId.APPerTurn);
            _move.IsValue = true;
        }

        private void OnStartTurn()
        {
            _effects.Next();
        }


        private void RedirectEvents(ReactiveEffect item, Operation operation)
        {
            actionThisChange?.Invoke(this, Operation.Change);
        }

        private int CurrentHPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxHP));
        private int CurrentAPCamp(int value) => Mathf.Clamp(value, 0, _abilities.GetValue(ActorAbilityId.MaxAP));
    }
}
