namespace Vurbiri.Colonization.Actors
{
    public class Warrior : Actor
    {

        public Warrior Init(WarriorSettings settings, int owner, ActorSkin skin, Hexagon startHex, GameplayEventBus eventBus)
        {
           base.Init(settings, owner, skin, startHex, eventBus);
            
            return this;
        }

        
    }
}
