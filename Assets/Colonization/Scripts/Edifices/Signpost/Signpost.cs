using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class Signpost : AEdifice
    {
        public override AEdifice Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice)
        {
            _graphic.Init(playerId, links);
            return this;
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> owner, bool isWall) => _graphic.AddRoad(linkId, owner);
    }
}
