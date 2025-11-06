using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class EmptyActionState : AActionState
            {
                public override bool CanUse { [Impl(256)] get => false; }

                public EmptyActionState(AStates<TActor, TSkin> parent, int id) : base(parent, id, 0) { }

                public override void Enter()
                {
                    GetOutToPrevState();
                }

                public override void Exit()
                {
                    signal.Send();
                }
            }
        }
    }
}
