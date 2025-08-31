namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon : Actor
    {
        protected override AStates StatesCreate(ActorSettings settings)
        {
            return new DemonStates(this, settings);
        }
    }
}
