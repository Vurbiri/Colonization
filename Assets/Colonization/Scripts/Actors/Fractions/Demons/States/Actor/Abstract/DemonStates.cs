using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        public abstract partial class ADemonStates<TSkin> : AStates<Demon, TSkin> where TSkin : ActorSkin
        {
            protected ADemonStates(Demon actor, ActorSettings settings) : base(actor, settings) { }
            sealed public override bool IsAvailable => _stateMachine.IsDefaultState;
        }

        public abstract partial class ADemonSpecMoveStates : ADemonStates<DemonSpecMoveSkin>
        {
            protected ADemonSpecMoveStates(Demon actor, ActorSettings settings) : base(actor, settings) { }
        }


        public partial class DemonStates : ADemonStates<DemonSkin>
        {
            public DemonStates(Demon demon, ActorSettings settings) : base(demon, settings)
            {
            }

            public override WaitSignal UseSpecSkill()
            {
                return null;
            }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float speedWalk, float speedRun) { }

            public override bool CanUseSpecSkill()
            {
                return false;
            }
        }
    }
}
