using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AHitEffect 
    {
        public abstract int Apply(Actor self, Actor target);
    }
}
