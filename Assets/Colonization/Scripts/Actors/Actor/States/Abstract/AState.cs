namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;

    public abstract partial class Actor
    {
        public class AState : ASelectableState
        {
            protected Actor _parent;

            public AState(Actor parent, int id = 0) : base(parent._stateMachine, id)
            {
                _parent = parent;
            }
        }
    }
}
