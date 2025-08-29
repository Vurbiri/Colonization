using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor
    {
        public abstract void AddSpecSkillState(int cost, int value);
        

        [MethodImpl(256)] public void AddMoveState(float speed) => _moveState = new(speed, this);
        [MethodImpl(256)] public void SetCountState(int count) => _skillState = new ASkillState[count];
        [MethodImpl(256)] public void AddSkillState(SkillSettings skill, float speedRun, int id)
        {
            _skillState[id] = ASkillState.Create(skill, speedRun, id, this);
        }

       

        public void Setup(ActorSettings settings, ActorInitData initData, Hexagon startHex)
        {
            _thisTransform = transform;
            _thisCollider = GetComponent<BoxCollider>();

            _typeId      = settings.TypeId;
            _id          = settings.Id;
            _owner       = initData.owner;
            _skin        = settings.InstantiateActorSkin(_owner, _thisTransform);
            _currentHex  = startHex;
            IsPersonTurn = false;
            Interactable = false;

            #region Bounds
            Bounds bounds = _skin.Bounds;
            _thisCollider.size = bounds.size;
            _thisCollider.center = bounds.center;

            _extentsZ = bounds.extents.z;
            #endregion

            #region Abilities
            _abilities = settings.Abilities;

            _currentHP   = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
            _currentAP   = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
            _move        = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);
            _profitMain  = _abilities.ReplaceToChance(ActorAbilityId.ProfitMain, _currentAP, _move);
            _profitAdv   = _abilities.ReplaceToChance(ActorAbilityId.ProfitAdv, _currentAP, _move);

            for (int i = 0; i < initData.buffs.Count; i++)
                _unsubscribers += initData.buffs[i].Subscribe(OnBuff);
            #endregion

            #region Effects
            _effects = new(_abilities);
            _currentHP.Subscribe(hp => { if (hp <= 0) Death(); });
            _effects.Subscribe(RedirectEvents);
            #endregion

            #region States
            _stateMachine.AssignDefaultState(new IdleState(this));
            settings.CreateStates(this);

            _skin.EventStart += _stateMachine.ToDefaultState;
            #endregion

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);
            gameObject.SetActive(true);
        }

        public void SetLoadData(ActorLoadData data)
        {
            _currentHP.Set(data.state.currentHP);
            _currentAP.Set(data.state.currentAP);
            _move.Set(data.state.move);

            for (int i = data.effects.Length - 1; i >= 0; i--)
                _effects.Add(data.effects[i]);

            PostLoad();
        }

        protected virtual void PostLoad() { }
    }
}
