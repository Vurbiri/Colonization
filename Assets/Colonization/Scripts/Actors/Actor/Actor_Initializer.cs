//Assets\Colonization\Scripts\Actors\Actor\Actor_Initializer.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;
    using static Vurbiri.Colonization.Characteristics.Skills;

    public abstract partial class Actor
    {
        public void AddMoveState(float speed) => _stateMachine.AddState(new MoveState(speed, this));
        public void AddBlockState(int cost, int value) => _stateMachine.AddState(_blockState = ABlockState.Create(_owner, cost, value, this));
        public void AddSkillState(IReadOnlyList<EffectsHit> effects, SkillSettings skill, float speedRun, int id)
        {
            _stateMachine.AddState(ASkillState.Create(effects, skill, speedRun, id, this));
        }

        public void Init(ActorSettings settings, BoxCollider collider, int owner, Hexagon startHex)
        {
            _typeId = settings.TypeId;
            _id = settings.Id;
            _owner = owner;
            _abilities = settings.Abilities;
            _skin = settings.InstantiateActorSkin(transform);
            _currentHex = startHex;

            _isPlayerTurn = owner == PlayerId.Player;

            _thisTransform = transform;
            _thisCollider = collider;

            Bounds bounds = _skin.Bounds;
            collider.size = bounds.size;
            collider.center = bounds.center;

            _extentsZ = bounds.extents.z;

            _eventBus = SceneServices.Get<GameplayEventBus>();
            _eventBus.EventStartTurn += OnStartTurn;

            _diplomacy = SceneObjects.Get<Diplomacy>();
            
            #region Effects
            _effects = new(_abilities);

            _currentHP = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
            _currentAP = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
            _move = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);

            _currentHP.Subscribe(hp => { if (hp <= 0) _deathCoroutine = StartCoroutine(Death_Coroutine()); });

            _effects.Subscribe(RedirectEvents);
            #endregion

            #region States
            Skills skills = settings.Skills;
            _stateMachine = new();
            _stateMachine.SetDefaultState(AIdleState.Create(this));
            _stateMachine.AddState(new TargetState());
            skills.CreateStates(this);
            #endregion

            _skin.EventStart += _stateMachine.ToDefaultState;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);
            gameObject.SetActive(true);
        }

        public void Load(ActorSettings settings, BoxCollider collider, int owner,  Hexagon startHex, ActorLoadData data)
        {
            Init(settings, collider, owner, startHex);

            _currentHP.Value = data.currentHP;
            _currentAP.Value = data.currentAP;
            _move.Value  = data.move;

            int count = data.effects.Length;
            for (int i = 0; i < count; i++)
                _effects.AddEffect(data.effects[i]);

            _isPlayerTurn = owner == PlayerId.Player & owner == data.currentPlayerId;

            if (_blockState.Enabled)
            {
                _skin.EventStart -= _stateMachine.ToDefaultState;
                _skin.EventStart += Block;
            }
        }
    }
}
