using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class RemoveLink
	{
        public readonly CrossroadLink link;
        public readonly bool isEnd;

        private RemoveLink(CrossroadLink link, bool isEnd)
        {
            this.link = link;
            this.isEnd = isEnd;

            link.RoadRemove();
        }

        [Impl(256)] public static RemoveLink Start(CrossroadLink link) => new(link, false);
        [Impl(256)] public static RemoveLink End(CrossroadLink link) => new(link, true);
    }
}
