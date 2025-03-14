//Assets\Colonization\Scripts\Actors\Actor\Actor_Initializer.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public virtual void AddMoveState(float speed) => _stateMachine.AddState(new MoveState(speed, this));
        public virtual void AddBlockState(int cost, int value) => _stateMachine.AddState(_blockState = ABlockState.Create(_owner, cost, value, this));
        public virtual void AddSkillState(IReadOnlyList<HitEffects> effects, SkillSettings skill, float speedRun, int id)
        {
            _stateMachine.AddState(ASkillState.Create(effects, skill, speedRun, id, this));
        }
        public virtual HitEffects[] AddSkillState(SkillSettings skill, float speedRun, int id)
        {
            HitEffects[] effects = skill.CreateEffectsHit(this, id);
            _stateMachine.AddState(ASkillState.Create(effects, skill, speedRun, id, this));
            return effects;
        }

        public void Init(ActorSettings settings, BoxCollider collider, Id<PlayerId> owner, IReactive<IPerk>[] buffs, Hexagon startHex)
        {
            _typeId = settings.TypeId;
            _id = settings.Id;
            _owner = owner;
            _skin = settings.InstantiateActorSkin(transform);
            _currentHex = startHex;

            #region Abilities
            _abilities = settings.Abilities;

            _currentHP = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
            _currentAP = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
            _move = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);

            for (int i = 0; i < buffs.Length; i++)
                _unsubscribers += buffs[i].Subscribe(OnBuff);
            #endregion

            _thisTransform = transform;
            _thisCollider = collider;

            #region Bounds
            Bounds bounds = _skin.Bounds;
            collider.size = bounds.size;
            collider.center = bounds.center;

            _extentsZ = bounds.extents.z;
            #endregion

            #region Get Services
            _eventBus = SceneServices.Get<GameplayEventBus>();
            _diplomacy = SceneServices.Get<Diplomacy>();

            var turn = SceneServices.Get<ITurn>();
            _unsubscribers += turn.Subscribe(OnNextTurn, false);
            _isPlayerTurn = owner == PlayerId.Player & owner == turn.CurrentId;
            #endregion

            #region Effects
            _effects = new(_abilities);
            _currentHP.Subscribe(hp => { if (hp <= 0) _deathCoroutine = StartCoroutine(Death_Cn()); });
            _effects.Subscribe(RedirectEvents);
            #endregion

            #region States
            Skills skills = settings.Skills;
            _stateMachine = new();
            _stateMachine.SetDefaultState(AIdleState.Create(this));
            _stateMachine.AddState(new BecomeTargetState());
            skills.CreateStates(this);
            #endregion

            _skin.EventStart += _stateMachine.ToDefaultState;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);
            gameObject.SetActive(true);
        }

        public void Load(ActorSettings settings, BoxCollider collider, Id<PlayerId> owner, IReactive<IPerk>[] buffs, Hexagon startHex, ActorLoadData data)
        {
            Init(settings, collider, owner, buffs, startHex);

            _currentHP.Value = data.currentHP;
            _currentAP.Value = data.currentAP;
            _move.Value  = data.move;

            int count = data.effects.Count;
            for (int i = 0; i < count; i++)
                _effects.AddEffect(data.effects[i]);

            if (_blockState.Enabled)
            {
                _skin.EventStart -= _stateMachine.ToDefaultState;
                _skin.EventStart += Block;
            }
        }
    }
}
