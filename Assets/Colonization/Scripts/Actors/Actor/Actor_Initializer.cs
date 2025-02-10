//Assets\Colonization\Scripts\Actors\Actor\Actor_Initializer.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public void Init(ActorSettings settings, BoxCollider collider, int owner, Hexagon startHex)
        {
            _typeId = settings.TypeId;
            _id = settings.Id;
            _owner = owner;
            _abilities = settings.Abilities;
            _skin = settings.InstantiateActorSkin(transform);
            _currentHex = startHex;

            _eventBus = SceneServices.Get<GameplayEventBus>();
            _eventBus.EventStartTurn += OnStartTurn;

            _diplomacy = SceneObjects.Get<Diplomacy>();

            _effects = new(_abilities);

            _currentHP = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
            _currentAP = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
            _move = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);

            _effects.Subscribe(RedirectEvents);

            _thisTransform = transform;
            Debug.Log("Выключить Collider на старте");
            _thisCollider = collider;
            _extentsZ = _thisCollider.bounds.extents.z;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);

            Skills skills = settings.Skills;
            _stateMachine = new();
            _stateMachine.SetDefaultState(AIdleState.Create(this));
            _stateMachine.AddState(skills.GetMoveState(this));
            _blockState = skills.GetBlockState(this);
            _stateMachine.AddState(_blockState);
            _stateMachine.AddState(new TargetState());
            _stateMachine.AddStates(skills.GetSkillSates(this));
            
            _skin.EventStart += _stateMachine.ToDefaultState;

            gameObject.SetActive(true);
        }

        public void Init(ActorSettings settings, BoxCollider collider, int owner, Hexagon startHex, ActorLoadData data)
        {
            Init(settings, collider, owner, startHex);

            _currentHP.Value = data.currentHP;
            _currentAP.Value = data.currentAP;
            _move.Value  = data.move;

            int count = data.effects.Length;
            for (int i = 0; i < count; i++)
                _effects.Add(data.effects[i]);

            if(_blockState.Enabled)
                _skin.EventStart += Block;
        }
    }
}
