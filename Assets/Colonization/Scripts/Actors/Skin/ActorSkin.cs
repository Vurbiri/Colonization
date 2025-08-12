using System;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [Space]
        [SerializeField] private AActorSFX _sfx;
        [SerializeField] private Animator _animator;
        [SerializeField] private SkinnedMeshRenderer _mesh;
        [ReadOnly, SerializeField] TimingSkillSettings[] _timings;
        [ReadOnly, SerializeField] float _durationDeath;

        #region CONST
        private const string B_IDLE = "bIdle", B_MOVE = "bMove", B_RUN = "bRun", B_BLOCK = "bBlock", B_DEATH = "bDeath";
        private static readonly string[] B_SKILLS = { "bSkill_0", "bSkill_1", "bSkill_2", "bSkill_3" };
        private const string T_REACT = "tReact";
        #endregion

        private Transform _thisTransform;
        
        private readonly StateMachine _stateMachine = new();
        private BoolSwitchState _moveState, _runState, _blockState;
        private SkillState[] _skillStates;
        private DeathState _deathState;
        private ReactState _reactState;

        public event Action EventStart;

        public Transform Transform => _thisTransform;
        public SkinnedMeshRenderer Mesh => _mesh;
        public Bounds Bounds => _bounds;
        public AActorSFX ActorSFX => _sfx;

        private void Start()
        {
            _reactState = new(this);
            _deathState = new(this, _durationDeath);

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public ActorSkin Init()
        {
            _thisTransform = transform;

            _stateMachine.SetDefaultState(new BoolSwitchState(B_IDLE, this));
            _moveState = new(B_MOVE, this);
            _runState = new(B_RUN, this);
            _blockState = new(B_BLOCK, this);

            int count = _timings.Length;
            _skillStates = new SkillState[count];
            for (int i = 0; i < count; i++)
                _skillStates[i] = new(B_SKILLS[i], this, _timings[i], i);

            _timings = null;

            return this;
        }
        
        public void Idle() => _stateMachine.ToDefaultState();

        public virtual void Block(bool isActive)
        {
            if (isActive) _stateMachine.SetState(_blockState);
            _sfx.Block(isActive);
        }

        public void Move() => _stateMachine.SetState(_moveState);

        public void Run() => _stateMachine.SetState(_runState);

        public virtual WaitSignal Skill(int index, ActorSkin targetActorSkin)
        {
            SkillState skill = _skillStates[index];
            skill.targetSkin = targetActorSkin;
            _stateMachine.SetState(skill);
            return skill.signal;
        }

        public void Impact(AudioClip clip)
        {
            _reactState.Repeat();
            _stateMachine.SetState(_reactState);

            if (clip != null)
                _sfx.Impact(clip);
        }

        public WaitStateSource<Actor.DeathStage> Death()
        {
            _stateMachine.SetState(_deathState);
            return _deathState.waitState;
        }

        public float GetFirsHitTime(int skillId) => _skillStates[skillId].waitHits[0].Time;

        #region Nested: TimingSkillSettings
        //*******************************************************
        [System.Serializable]
        protected class TimingSkillSettings
        {
            public float[] hitTimes;
            public float remainingTime;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _animator);
            this.SetComponent(ref _sfx);
            this.SetChildren(ref _mesh);

            if (_animator != null)
                _durationDeath = ((AnimatorOverrideController)_animator.runtimeAnimatorController)[A_DEATH].length;
        }
#endif
    }
}
