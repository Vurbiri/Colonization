namespace Vurbiri.Colonization
{
    sealed public partial class Warrior : Actor
    {
        protected override AStates StatesCreate(ActorSettings settings)
        {
            return new WarriorStates(this, settings);
        }
    }
}
