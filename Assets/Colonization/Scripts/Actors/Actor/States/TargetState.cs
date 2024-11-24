//Assets\Colonization\Scripts\Actors\Actor\States\TargetState.cs
using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        private class TargetState : ASelectableState
        {
            public TargetState() : base(null, 0)
            {
            }
        }
    }
}
