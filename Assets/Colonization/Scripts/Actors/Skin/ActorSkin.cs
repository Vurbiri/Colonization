using System;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.FSM;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [Space]
        [SerializeField] private AActorSFX _sfx;
        [SerializeField] private Animator _animator;
        [ReadOnly, SerializeField] private float _durationDeath;

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

        public Transform Transform { [Impl(256)] get => _thisTransform; }
        public Bounds Bounds { [Impl(256)] get => _bounds; }
        public AActorSFX ActorSFX { [Impl(256)] get => _sfx; }

        private void Start()
        {
            // тут, потому что в них используется GetBehaviour<T>()
            _reactState = new(this);
            _deathState = new(this, _durationDeath);

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public ActorSkin Init(Id<PlayerId> owner, Skills skills)
        {
            _thisTransform = transform;
            
            if (owner != PlayerId.Satan)
                GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = GameContainer.Materials[owner].materialWarriors;

            _stateMachine.AssignDefaultState(new BoolSwitchState(B_IDLE, this));
            _moveState  = new(B_MOVE,  this);
            _runState   = new(B_RUN,   this);
            _blockState = new(B_BLOCK, this);

            var timings = skills.Timings;
            _skillStates = new SkillState[timings.Count];
            for (int i = 0; i < timings.Count; i++)
                _skillStates[i] = new(B_SKILLS[i], this, timings[i], i);

            _sfx.Init(skills.HitSfxNames);

            return this;
        }

        [Impl(256)] public void Idle() => _stateMachine.ToDefaultState();

        [Impl(256)] public virtual void Block(bool isActive)
        {
            if (isActive)
                _stateMachine.SetState(_blockState);
            _sfx.Block(isActive);
        }

        [Impl(256)] public void Move() => _stateMachine.SetState(_moveState);
        [Impl(256)] public void Run() => _stateMachine.SetState(_runState);

        [Impl(256)] public WaitSignal Skill(int index, ActorSkin targetActorSkin)
        {
            SkillState skill = _skillStates[index];
            _stateMachine.SetState(skill);

            return skill.Setup(targetActorSkin);
        }

        [Impl(256)] public void Impact(AudioClip clip)
        {
            _reactState.Repeat();
            _stateMachine.SetState(_reactState, true);

            if (clip != null)
                _sfx.Impact(clip);
        }

        [Impl(256)] public WaitStateSource<Actor.DeathStage> Death()
        {
            _stateMachine.ForceSetState(_deathState);
            return _deathState.waitState;
        }

        [Impl(256)] public float GetFirsHitTime(int skillId) => _skillStates[skillId].FirsHitTime;


#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _animator);
            this.SetComponent(ref _sfx);

            if (_animator != null)
                _durationDeath = ((AnimatorOverrideController)_animator.runtimeAnimatorController)["A_Death"].length;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }
#endif
    }
}
