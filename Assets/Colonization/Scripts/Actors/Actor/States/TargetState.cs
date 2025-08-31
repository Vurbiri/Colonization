using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class TargetState : ASelectableState
            {
                public TargetState() : base(null)
                {
                }
            }
        }
    }
}
