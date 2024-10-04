namespace Vurbiri.Colonization
{
    public class Signpost : AEdifice
    {
        public override void Setup(AEdifice edifice, IdHashSet<LinkId, CrossroadLink> links)
        {
            _graphic.Initialize(_owner, links);

            _prefabUpgrade = edifice;
            _nextId = edifice.Id.ToInt;
            _nextGroupId = edifice.GroupId;
            _isUpgrade = true;
        }

        public override bool Build(AEdifice prefab, PlayerType owner, IdHashSet<LinkId, CrossroadLink> links, bool isWall, out AEdifice city)
        {
            _prefabUpgrade = prefab;
            _isWall = isWall;
            return Upgrade(owner, links, out city);
        }

        public override bool Upgrade(PlayerType owner, IdHashSet<LinkId, CrossroadLink> links, out AEdifice city)
        {
            _owner = owner;
            if (base.Upgrade(owner, links, out city))
                return true;

            _owner = PlayerType.None;
            return false;
        }

        public override void AddRoad(Id<LinkId> linkId, PlayerType owner) => _graphic.AddRoad(linkId, owner);

        public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
    }
}
