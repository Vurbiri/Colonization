namespace Vurbiri.Colonization
{
    public class Signpost : AEdifice
    {
        public override void Setup(AEdifice edifice, IdHashSet<LinkId, CrossroadLink> links)
        {
            _graphic.Init(_owner, links);

            _prefabUpgrade = edifice;
            _nextId = edifice.Id.ToInt;
            _nextGroupId = edifice.GroupId;
            _isUpgrade = true;

            SetCost();
        }

        public override bool Build(AEdifice prefab, Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links, bool isWall, out AEdifice city)
        {
            _prefabUpgrade = prefab;
            _isWall = isWall;
            return Upgrade(playerId, links, out city, out _);
        }

        public override bool Upgrade(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links, out AEdifice city, out ACurrencies cost)
        {
            _owner = playerId;
            if (base.Upgrade(playerId, links, out city, out cost))
                return true;

            _owner = PlayerId.None;
            return false;
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) => _graphic.AddRoad(linkId, playerId);

        public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
    }
}
