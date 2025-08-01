using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public void AddMoveState(float speed) => _moveState = new(speed, this);
        public void AddBlockState(int cost, int value) => _blockState = new(cost, value, this);
        public void SetCountState(int count) => _skillState = new ASkillState[count];
        public void AddSkillState(IReadOnlyList<HitEffects> effects, SkillSettings skill, float speedRun, int id)
        {
            _skillState[id] = ASkillState.Create(effects, skill, speedRun, id, this);
        }
        public HitEffects[] AddSkillState(SkillSettings skill, float speedRun, int id)
        {
            HitEffects[] effects = skill.CreateEffectsHit(this, id);
            _skillState[id] = ASkillState.Create(effects, skill, speedRun, id, this);
            return effects;
        }

        public void Setup(ActorSettings settings, ActorInitData initData, BoxCollider collider, Hexagon startHex)
        {
            _thisTransform = transform;
            _thisCollider  = collider;

            _typeId      = settings.TypeId;
            _id          = settings.Id;
            _owner       = initData.owner;
            _skin        = settings.InstantiateActorSkin(transform);
            _currentHex  = startHex;
            IsPersonTurn = false;
            Interactable = false;

            #region Abilities
            _abilities   = settings.Abilities;

            _currentHP   = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
            _currentAP   = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
            _move        = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);
            _profitMain  = _abilities.ReplaceToChance(ActorAbilityId.ProfitMain, _currentAP, _move);
            _profitAdv   = _abilities.ReplaceToChance(ActorAbilityId.ProfitAdv, _currentAP, _move);

            for (int i = 0; i < initData.buffs.Length; i++)
                _unsubscribers += initData.buffs[i].Subscribe(OnBuff);
            #endregion

            #region Bounds
            Bounds bounds = _skin.Bounds;
            collider.size = bounds.size;
            collider.center = bounds.center;

            _extentsZ = bounds.extents.z;
            #endregion

            #region Effects
            _effects = new(_abilities);
            _currentHP.Subscribe(hp => { if (hp <= 0) Death(); });
            _effects.Subscribe(RedirectEvents);
            #endregion

            #region States
            _stateMachine.SetDefaultState(new IdleState(this));
            settings.Skills.CreateStates(this);

            _skin.EventStart += _stateMachine.ToDefaultState;
            #endregion

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);
            gameObject.SetActive(true);
        }

        public void Load(ActorSettings settings, ActorInitData initData, BoxCollider collider, Hexagon startHex, ActorLoadData data)
        {
            Setup(settings, initData, collider, startHex);

            _currentHP.Set(data.state.currentHP);
            _currentAP.Set(data.state.currentAP);
            _move.Set(data.state.move);

            for (int i = data.effects.Length - 1; i >= 0; i--)
                _effects.Add(data.effects[i]);

            if (_blockState.IsApplied)
            {
                _skin.EventStart -= _stateMachine.ToDefaultState;
                _skin.EventStart += Block;
            }
        }
    }
}
