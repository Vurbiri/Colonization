namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon : Actor
    {
        protected override AStates StatesCreate(ActorSettings settings)
        {
            if(_id == DemonId.Imp)
                return new ImpStates(this, settings);

            return new DemonStates(this, settings);
        }
    }
}
