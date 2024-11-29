//Assets\Colonization\Scripts\Actors\Skin\ActorSkin.cs
using System;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour, IDisposable
    {
        [SerializeField] private AActorSFX _sfx;
        [SerializeField] private Animator _animator;
        [SerializeField] private TimingSkillSettings[] _timings = new TimingSkillSettings[COUNT_SKILLS];

        #region CONST
        private const string T_IDLE = "tIdleAdv", B_IDLE = "bIdle";
        private const string B_MOVE = "bMove", B_RUN = "bRun", B_BLOCK = "bBlock";
        private const string T_REACT = "tReact", T_DEATH = "tDeath";
        private static readonly string[] T_SKILLS = { "bSkill_0", "bSkill_1", "bSkill_2", "bSkill_3" };
        public const int COUNT_SKILLS = 4;
        #endregion

        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0;

        private BoolSwitchState _moveState, _runState, _blockState;
        private readonly SkillState[] _skillStates = new SkillState[COUNT_SKILLS];
        private ATriggerSwitchState _reactState, _deathState;

        public event Action EventStart;

        private void Start()
        {
            _stateMachine.SetDefaultState(CreateBoolState(B_IDLE));

            _moveState  = CreateBoolState(B_MOVE);
            _runState   = CreateBoolState(B_RUN);
            _blockState = CreateBoolState(B_BLOCK);

            _reactState = new ReactState(this);
            _deathState = new DeathState(this);

            for (int i = 0; i < COUNT_SKILLS; i++)
                _skillStates[i] = new(T_SKILLS[i], this, _timings[i], i);
            _timings = null;

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public virtual void Idle() => _stateMachine.ToDefaultState();

        public virtual void Block() => _stateMachine.SetState(_blockState);

        public virtual void Move() => _stateMachine.SetState(_moveState);

        public virtual void Run() => _stateMachine.SetState(_runState);

        public virtual WaitActivate Skill(int index, Transform target)
        {
            SkillState skill = _skillStates[index];
            skill.target = target;
            _stateMachine.SetState(skill);
            return skill.waitActivate;
        }

        public WaitActivate React()
        {
            _stateMachine.SetState(_reactState);
            return _reactState.waitActivate;
        }

        public WaitActivate Death()
        {
            _stateMachine.SetState(_deathState);
            return _deathState.waitActivate;
        }

        private BoolSwitchState CreateBoolState(string nameParam) => new(nameParam, this, _idBoolState++);

        public void Dispose()
        {
            _stateMachine.Dispose();
            for (int i = 0; i < COUNT_SKILLS; i++)
                _skillStates[i].Dispose();
            _moveState.Dispose();
            _runState.Dispose();
            _blockState.Dispose();
        }

#if UNITY_EDITOR

        private static readonly string[] SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };

        public void SetAnimationClip(AnimationClipSettingsScriptable clipSettings, int id)
        {
            var timing = _timings[id];
            float[] damageTimes = clipSettings.damageTimes;
            timing.damageTimes = new float[damageTimes.Length];
            for (int i = 0; i < damageTimes.Length; i++)
                timing.damageTimes[i] = damageTimes[i];
            timing.remainingTime = clipSettings.RemainingTime;

            AnimatorOverrideController animator = (AnimatorOverrideController)_animator.runtimeAnimatorController;
            if (animator[SKILLS[id]] != clipSettings.clip)
                animator[SKILLS[id]] = clipSettings.clip;
        }

        private void OnValidate()
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
            if (_sfx == null)
                _sfx = GetComponent<AActorSFX>();

            if(_timings != null && _timings.Length != COUNT_SKILLS)
                Array.Resize(ref _timings, COUNT_SKILLS);
        }
#endif
    }
}
