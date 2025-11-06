using Vurbiri.Colonization.FSMSelectable;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract class Actions
        {
            public abstract bool CanUseSkill(int id);
            public abstract bool CanUseMainSkill(int id);
            public abstract bool CanUseSpecSkill();
            public abstract bool CanUseMoveSkill();

            public abstract SkillCode GetSkillCode(int id);

            public abstract WaitSignal UseSkill(int id);
            public abstract WaitSignal UseSpecSkill();
            public abstract WaitSignal UseMoveSkill();
            
            public abstract WaitStateSource<DeathStage> Death();
        }
        //============================================================================
        public abstract class AStates : Actions
        {
            private readonly TargetState _targetState = new();
            protected readonly StateMachineSelectable _stateMachine = new();

            public int MainSkillsCount { [Impl(256)] get; [Impl(256)] set; }
            public bool IsDefault { [Impl(256)] get => _stateMachine.IsDefaultState; }

            public abstract ActorSkin Skin { get; }
            public abstract ActorSFX SFX { get; }
            public abstract bool IsAvailable { get; }

            [Impl(256)] public void ToDefault() => _stateMachine.ToDefaultState();

            [Impl(256)] public void Cancel() => _stateMachine.Cancel();
            [Impl(256)] public void Select(MouseButton button) => _stateMachine.Select(button);
            [Impl(256)] public void Unselect(ISelectable newSelectable) => _stateMachine.Unselect(newSelectable);

            [Impl(256)] public bool ToTarget() => _stateMachine.SetState(_targetState, true);
            [Impl(256)] public bool FromTarget() => _stateMachine.GetOutToPrevState(_targetState);

            public virtual void Load() { }
        }
        //============================================================================
        public abstract partial class AStates<TActor, TSkin> : AStates where TActor : Actor where TSkin : ActorSkin
        {
            protected readonly TActor _actor;
            protected readonly TSkin _skin;

            protected readonly AActionState[] _actionSkills = new AActionState[CONST.ACTION_SKILLS_COUNT];
            protected MoveState _moveState;
            protected DeathState _deathState;

            sealed public override ActorSkin Skin => _skin;
            sealed public override ActorSFX SFX => _skin.SFX;

            protected AStates(TActor actor, ActorSettings settings)
            {
                _actor = actor;
                _skin = settings.InstantiateSkin<TSkin>(actor._owner, actor._thisTransform);

                _stateMachine.AssignDefaultState(new IdleState(this));
                settings.Skills.CreateStates(this);

                _skin.EventStart += _stateMachine.ToDefaultState;
            }

            sealed public override bool CanUseSkill(int id) => (id >= 0 & id < CONST.ACTION_SKILLS_COUNT) && _actionSkills[id].CanUse;
            sealed public override bool CanUseMainSkill(int id) => (id >= 0 & id < CONST.MAIN_SKILLS_COUNT) && _actionSkills[id].CanUse;
            sealed public override bool CanUseMoveSkill() => _moveState.CanUse;

            sealed public override SkillCode GetSkillCode(int id) => _actionSkills[id].code;

            sealed public override WaitSignal UseMoveSkill()
            {
                _stateMachine.SetState(_moveState, true);
                return _moveState.signal.Restart();
            }
            sealed public override WaitSignal UseSkill(int id)
            {
                _stateMachine.SetState(_actionSkills[id], true);
                return _actionSkills[id].signal.Restart();
            }

            sealed public override WaitStateSource<DeathStage> Death()
            {
                _stateMachine.ForceSetState(_deathState = new(this), true);
                return _deathState.stage;
            }

            [Impl(256)] public void AddMoveSkillState(float speed) => _actionSkills[CONST.MOVE_SKILL_ID] = _moveState = new(speed, this);
            [Impl(256)] public void AddSkillState(SkillSettings skill, float speedRun, int id) => _actionSkills[id] = ASkillState.Create(skill, speedRun, id, this);
            [Impl(256)] public void AddEmptySkillState(int id) => _actionSkills[id] = new EmptyActionState(this, id);

            public abstract void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed);
        }
    }
}
