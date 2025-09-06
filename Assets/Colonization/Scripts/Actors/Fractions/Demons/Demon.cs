namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon : Actor
    {
        protected override AStates StatesCreate(ActorSettings settings)
        {
            if(_id == DemonId.Imp)
                return new ImpStates(this, settings);

            if (_id == DemonId.Bomb)
                return new BombStates(this, settings);

                if (_id == DemonId.Grunt)
            return new GruntStates(this, settings);



            return new DemonStates(this, settings);
        }
    }
}