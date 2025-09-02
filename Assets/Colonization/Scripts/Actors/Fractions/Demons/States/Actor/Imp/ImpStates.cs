using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class ImpStates : ADemonSpecMoveStates
        {
            private FearState _fearState;

            public ImpStates(Demon actor, ActorSettings settings) : base(actor, settings) { }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float speedWalk, float speedRun)
            {
                _fearState = new(specSkill, speedWalk, this);
            }

            public override bool CanUseSpecSkill() => _fearState.CanUse;

            public override WaitSignal UseSpecSkill()
            {
                _stateMachine.SetState(_fearState, true);
                return _fearState.signal.Restart();
            }
        }
    }
}
