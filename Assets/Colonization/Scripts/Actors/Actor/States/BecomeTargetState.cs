//Assets\Colonization\Scripts\Actors\Actor\States\BecomeTargetState.cs
using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class BecomeTargetState : ASelectableState
        {
            public BecomeTargetState() : base(null)
            {
            }
        }
    }
}
