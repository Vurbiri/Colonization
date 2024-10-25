using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class Signpost : AEdifice
    {
        public override void Setup(AEdifice edifice, IReadOnlyList<CrossroadLink> links)
        {
            _graphic.Init(_owner, links);

            _prefabUpgrade = edifice;
            _nextId = edifice.Id.Value;
            _nextGroupId = edifice.GroupId;
            _isUpgrade = true;
        }

        public override bool Build(AEdifice prefab, Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isWall, out AEdifice city)
        {
            _prefabUpgrade = prefab;
            _isWall = isWall;
            return Upgrade(playerId, links, out city);
        }

        public override bool Upgrade(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, out AEdifice city)
        {
            _owner = playerId;
            if (base.Upgrade(playerId, links, out city))
                return true;

            _owner = PlayerId.None;
            return false;
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> owner) => _graphic.AddRoad(linkId, owner);

    }
}
