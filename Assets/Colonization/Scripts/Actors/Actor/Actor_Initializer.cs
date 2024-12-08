//Assets\Colonization\Scripts\Actors\Actor\Actor_Initializer.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public void Init(ActorSettings settings, int owner, Hexagon startHex)
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

            _currentHP = _abilities.GetAbility(ActorAbilityId.CurrentHP);
            _currentHP.Clamp = CurrentHPCamp;
            _currentHP.Value = _abilities.GetValue(ActorAbilityId.MaxHP);

            _currentAP = _abilities.GetAbility(ActorAbilityId.CurrentAP);
            _currentAP.Clamp = CurrentAPCamp;

            _move = _abilities.GetAbility(ActorAbilityId.IsMove);

            _effects.Subscribe(RedirectEvents);

            _thisTransform = transform;
            Debug.Log("Выключить Collider на старте");
            _thisCollider = GetComponent<Collider>();
            _extentsZ = _thisCollider.bounds.extents.z;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);

            Skills skills = settings.Skills;

            _stateMachine = new();
            Debug.Log("разкомментить PlayerIdleState");
            //AIdleState idle = owner == PlayerId.Player ? new PlayerIdleState(this) : new AIIdleState(this);

            _stateMachine.SetDefaultState(new PlayerIdleState(this));
            _stateMachine.AddState(skills.GetMoveSate(this));
            _blockState = skills.GetBlockState(this);
            _stateMachine.AddState(_blockState);
            _stateMachine.AddStates(skills.GetSkillSates(this));
            _stateMachine.AddState(new TargetState());

            _skin.EventStart += _stateMachine.ToDefaultState;

            gameObject.SetActive(true);
        }

        public void Init(ActorSettings settings, int owner, Hexagon startHex, ActorLoadData data)
        {
            Init(settings, owner, startHex);

            _currentHP.Value = data.currentHP;
            _currentAP.Value = data.currentAP;
            _move.Value      = data.move;

            int count = data.effects.Length;
            for (int i = 0; i < count; i++)
                _effects.Add(data.effects[i]);

            if(data.isBlock)
                _skin.EventStart += Block;
        }
    }
}
