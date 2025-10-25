namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract class AI<T> where T : AStates
        {
            protected readonly Actor _actor;
            protected readonly T _action;

            protected AI(Actor actor)
            {
                _actor = actor;
                _action = (T)actor._states;
            }
        }
    }
}
