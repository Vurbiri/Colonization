using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.FSM;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    //[DisallowMultipleComponent]
    public abstract partial class ActorSkin : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [Space]
        [SerializeField] protected Animator _animator;
        [ReadOnly, SerializeField] private WaitRealtime _durationDeath;

        #region Static
        private static readonly int s_idIdle = Animator.StringToHash("bIdle"), s_idDeath = Animator.StringToHash("bDeath");
        private static readonly int s_idMove = Animator.StringToHash("bWalk"), s_idRun = Animator.StringToHash("bRun");
        private static readonly int[] s_idSkills = 
            { Animator.StringToHash("bSkill_0"), Animator.StringToHash("bSkill_1"), Animator.StringToHash("bSkill_2"), Animator.StringToHash("bSkill_3") };
        private static readonly int s_idReact = Animator.StringToHash("tReact");
        #endregion

        protected Transform _thisTransform;
        protected ActorSFX _sfx;

        protected readonly StateMachine _stateMachine = new();
        protected BoolSwitchState _moveState, _runState;
        private SkillState[] _skillStates;
        private DeathState _deathState;
        private ReactState _reactState;

        public event Action EventStart;

        public ActorSFX SFX { [Impl(256)] get => _sfx; }
        public Vector3 Size { [Impl(256)] get => _bounds.size; }
        public Vector3 Extents { [Impl(256)] get => _bounds.extents; }

        private void Start()
        {
            // тут, потому что в них используется GetBehaviour<T>()
            _reactState = new(this);
            _deathState = new(this);

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public abstract void Init(Id<PlayerId> owner, Skills skills);

        [Impl(256)] protected void InitInternal(ReadOnlyArray<AnimationTime> timings, ActorSFX sfx)
        {
            _thisTransform = transform;
            _sfx = sfx;

            _stateMachine.AssignDefaultState(new BoolSwitchState(s_idIdle, this));
            _moveState = new(s_idMove, this);
            _runState = new(s_idRun, this);

            _skillStates = new SkillState[timings.Count];
            for (int i = 0; i < timings.Count; i++)
                _skillStates[i] = new(s_idSkills[i], this, timings[i], i);
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

        [Impl(256)] public void Impact(AudioClip clip)
        {
            _reactState.Setup(clip);
            _stateMachine.SetState(_reactState, true);
        }

        [Impl(256)] public WaitStateSource<Actor.DeathStage> Death()
        {
            _stateMachine.ForceSetState(_deathState);
            return _deathState.waitState;
        }

        [Impl(256)] public float GetFirsHitTime(int skillId) => _skillStates[skillId].FirsHitTime;

        [Impl(256)] public void Play(AudioClip clip) => _sfx.Play(clip);
        [Impl(256)] public Vector3 GetPosition(float heightRate)
        {
            Vector3 position = _thisTransform.position;
            position.y += _bounds.extents.y * heightRate;
            return position;
        }

        [Impl(256)] public float SetupCollider(BoxCollider collider)
        {
            collider.size = _bounds.size;
            collider.center = _bounds.center;
            return _bounds.extents.z;
        }

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

        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
            Gizmos.DrawSphere(_bounds.center, 0.2f);
        }

        //public void OnDrawGizmos()
        //{
        //    Gizmos.matrix = Matrix4x4.identity;
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        //    Gizmos.DrawSphere(_bounds.center, 0.2f);
        //}
#endif
    }
}
