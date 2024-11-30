//Assets\Colonization\Scripts\Actors\Skin\ActorSkin.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour, IDisposable
    {
        [SerializeField] private AActorSFX _sfx;
        [SerializeField] private Animator _animator;
        [HideInInspector, SerializeField] private int  _count;
        [HideInInspector, SerializeField] TimingSkillSettings[] _timings;

        #region CONST
        private const string B_IDLE = "bIdle";
        private const string B_MOVE = "bMove", B_RUN = "bRun", B_BLOCK = "bBlock";
        private const string T_REACT = "tReact", T_DEATH = "tDeath";
        private static readonly string[] T_SKILLS = { "bSkill_0", "bSkill_1", "bSkill_2", "bSkill_3" };
        #endregion

        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0;

        private BoolSwitchState _moveState, _runState, _blockState;
        private SkillState[] _skillStates;
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

            _skillStates = new SkillState[_count];
            for (int i = 0; i < _count; i++)
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
            for (int i = 0; i < _count; i++)
                _skillStates[i].Dispose();
            _moveState.Dispose();
            _runState.Dispose();
            _blockState.Dispose();
        }

#if UNITY_EDITOR

        private static readonly string[] SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };

        public void SetAnimationClip(AnimationClipSettingsScriptable clipSettings, int id)
        {

            AnimatorOverrideController animator = (AnimatorOverrideController)_animator.runtimeAnimatorController;
            if (animator[SKILLS[id]] != clipSettings.clip)
                animator[SKILLS[id]] = clipSettings.clip;

            if (clipSettings.damageTimes == null || clipSettings.damageTimes.Length == 0)
                return;

            var timing = _timings[id];
            int count = clipSettings.damageTimes.Length;
            timing.damageTimes = new float[count];
            float totalTime = clipSettings.totalTime;
            float current, prev = timing.damageTimes[0] = totalTime * clipSettings.damageTimes[0] / 100f;
            for (int i = 1; i < count; i++)
            {
                current = totalTime * clipSettings.damageTimes[i] / 100f;
                timing.damageTimes[i] = current - prev;
                prev = current;
            }

            timing.remainingTime = clipSettings.totalTime - prev;
        }

        public void SetCountAnimationClips(int count)
        {
            _count = count;
            _timings ??= new TimingSkillSettings[count];
            if (_timings.Length != count)
                Array.Resize(ref _timings, count);

            AnimatorOverrideController animator = (AnimatorOverrideController)_animator.runtimeAnimatorController;
            for (int i = 1; i < Skills.COUNT_SKILLS_MAX; i++)
                animator[SKILLS[i]] = null;
        }

        private void OnValidate()
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
            if (_sfx == null)
                _sfx = GetComponent<AActorSFX>();

            _timings ??= new TimingSkillSettings[_count];
            if (_timings.Length != _count)
                Array.Resize(ref _timings, _count);

        }
#endif
    }
}
