namespace Vurbiri.Colonization.Actors
{
    public class EmptySFX : IHitSFX
    {
        public const string NAME = "Empty";

        public System.Collections.IEnumerator Hit(IUserSFX user, ActorSkin target) => null;
    }
}
