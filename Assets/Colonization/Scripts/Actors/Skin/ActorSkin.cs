using System;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.FSM;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    [DisallowMultipleComponent]
    public abstract partial class ActorSkin : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [Space]
        [SerializeField] private Animator _animator;
        [ReadOnly, SerializeField] private float _durationDeath;

        #region CONST
        private const string B_IDLE = "bIdle", B_MOVE = "bWalk", B_RUN = "bRun", B_DEATH = "bDeath", B_SPEC = "bWalk_Spec";
        private static readonly string[] B_SKILLS = { "bSkill_0", "bSkill_1", "bSkill_2", "bSkill_3" };
        private const string T_REACT = "tReact";
        #endregion

        protected Transform _thisTransform;
        protected ActorSFX _sfx;

        protected readonly StateMachine _stateMachine = new();
        protected BoolSwitchState _moveState, _runState, _specState;
        private SkillState[] _skillStates;
        private DeathState _deathState;
        private ReactState _reactState;

        public event Action EventStart;

        public Transform Transform { [Impl(256)] get => _thisTransform; }
        public Bounds Bounds { [Impl(256)] get => _bounds; }
        public ActorSFX ActorSFX { [Impl(256)] get => _sfx; }


        private void Start()
        {
            // тут, потому что в них используется GetBehaviour<T>()
            _reactState = new(this);
            _deathState = new(this, _durationDeath);

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public abstract void Init(Id<PlayerId> owner, Skills skills);

        protected void Init(Skills skills, ActorSFX sfx)
        {
            sfx.Init(skills.HitSfxNames);

            _thisTransform = transform;
            _sfx = sfx;

            _stateMachine.AssignDefaultState(new BoolSwitchState(B_IDLE, this));
            _moveState  = new(B_MOVE,  this);
            _runState   = new(B_RUN,   this);
            _specState  = new(B_SPEC,  this);

            var timings = skills.Timings;
            _skillStates = new SkillState[timings.Count];
            for (int i = 0; i < timings.Count; i++)
                _skillStates[i] = new(B_SKILLS[i], this, timings[i], i);
        }

        [Impl(256)] public void Idle() => _stateMachine.ToDefaultState();
        [Impl(256)] public void Move() => _stateMachine.SetState(_moveState);
        [Impl(256)] public void Run() => _stateMachine.SetState(_runState);

        [Impl(256)] public WaitSignal Skill(int index, ActorSkin targetActorSkin)
        {
            SkillState skill = _skillStates[index];
            _stateMachine.SetState(skill);

            return skill.Setup(targetActorSkin);
        }

        [Impl(256)] public void SpecSkill() => _stateMachine.SetState(_specState);

        [Impl(256)] public void Impact(AudioClip clip)
        {
            _reactState.Impact(clip);
            _stateMachine.SetState(_reactState, true);
        }

        [Impl(256)] public WaitStateSource<Actor.DeathStage> Death()
        {
            _stateMachine.ForceSetState(_deathState);
            return _deathState.waitState;
        }

        [Impl(256)] public float GetFirsHitTime(int skillId) => _skillStates[skillId].FirsHitTime;


#if UNITY_EDITOR

        private UnityEditor.Animations.AnimatorState _state;
        
        private void OnValidate()
        {
            this.SetComponent(ref _animator);

            if (_animator != null && _animator.runtimeAnimatorController != null)
            {
                var overrideController = (AnimatorOverrideController)_animator.runtimeAnimatorController;
                if(_state == null) _state = GetState(overrideController.runtimeAnimatorController);
                _durationDeath = overrideController["A_Death"].length * _state.speed * 0.975f;

                // Local
                static UnityEditor.Animations.AnimatorState GetState(RuntimeAnimatorController controller)
                {
                    foreach(var cState in ((UnityEditor.Animations.AnimatorController)controller).layers[0].stateMachine.states)
                        if(cState.state.name == "Death")
                            return cState.state;

                    return new();
                }
            }
        }

        //public void OnDrawGizmosSelected()
        //{
        //    Gizmos.matrix = Matrix4x4.identity;
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        //}

        public void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
            Gizmos.DrawSphere(_bounds.center, 0.2f);
        }
#endif
    }
}
