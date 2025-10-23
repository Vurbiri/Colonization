namespace Vurbiri.Colonization
{
    public class EmptySFX : ISFX
    {
        public const string NAME = "Empty";

        public System.Collections.IEnumerator Run(ActorSFX user, ActorSkin target) => null;
    }
}
