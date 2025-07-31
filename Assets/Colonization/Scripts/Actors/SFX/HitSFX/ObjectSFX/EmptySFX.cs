namespace Vurbiri.Colonization.Actors
{
    public class EmptySFX : IHitSFX
    {
        public const string NAME = "Empty";

        public System.Collections.IEnumerator Hit(ISFXUser user, ActorSkin target) => null;
    }
}
