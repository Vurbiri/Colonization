//Assets\Colonization\Scripts\Actors\Actor\States\TargetState.cs
using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected class TargetState : ASelectableState
        {
            public TargetState() : base(null)
            {
            }
        }
    }
}
