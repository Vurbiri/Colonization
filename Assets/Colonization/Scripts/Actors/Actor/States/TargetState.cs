using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class TargetState : ASelectableState
        {
            public TargetState() : base(null)
            {
            }
        }
    }
}
