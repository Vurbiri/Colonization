//Assets\Colonization\Scripts\Actors\Actor\Actor_Initializer.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public void Init(ActorSettings settings, int owner, Hexagon startHex, GameplayEventBus eventBus)
        {
            _typeId = settings.TypeId;
            _id = settings.Id;
            _owner = owner;
            _abilities = settings.Abilities;
            _skin = settings.InstantiateActorSkin(transform);
            _currentHex = startHex;
            _eventBus = eventBus;

            _effects = new(_abilities);

            _currentHP = _abilities.GetAbility(ActorAbilityId.CurrentHP);
            _currentHP.Clamp = CurrentHPCamp;
            _currentHP.Value = _abilities.GetValue(ActorAbilityId.MaxHP);

            _currentAP = _abilities.GetAbility(ActorAbilityId.CurrentAP);
            _currentAP.Clamp = CurrentAPCamp;

            _move = _abilities.GetAbility(ActorAbilityId.IsMove);

            _thisTransform = transform;
            _extentsZ = GetComponent<BoxCollider>().bounds.extents.z;
            _effects.Subscribe(RedirectEvents);

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);

            Skills skills = settings.Skills;

            Debug.Log("разкомментить PlayerIdleState");
            //AIdleState idle = owner == PlayerId.Player ? new PlayerIdleState(this) : new AIIdleState(this);
            AIdleState idle = new PlayerIdleState(this);

            _stateMachine.AddState(idle);
            _stateMachine.SetDefaultState(idle);

            _stateMachine.AddState(skills.GetMoveSate(this));

            _blockState = skills.GetBlockState(this);
            _skillStates = skills.GetSkillSates(this);

            _skin.EventStart += _stateMachine.ToDefault;

            gameObject.SetActive(true);
        }

        public void Init(ActorSettings settings, int owner, Hexagon startHex, ActorLoadData data, GameplayEventBus eventBus)
        {
            Init(settings, owner, startHex, eventBus);

            _currentHP.Value = data.currentHP;
            _currentAP.Value = data.currentAP;
            _move.Value    = data.move;

            int count = data.effects.Length;
            for (int i = 0; i < count; i++)
                _effects.Add(data.effects[i]);

            if(data.isBlock)
                _skin.EventStart += Block;
        }
    }
}
