namespace Vurbiri.Colonization
{
    public class Signpost : AEdifice
    {
        public override void Setup(AEdifice edifice, EnumHashSet<LinkType, CrossroadLink> links)
        {
            _graphic.Initialize(_owner, links);

            _prefabUpgrade = edifice;
            _idNext = edifice.Id;
            _idGroupNext = edifice.IdGroup;
            _isUpgrade = true;
        }

        public override bool Build(AEdifice prefab, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, bool isWall, out AEdifice city)
        {
            _prefabUpgrade = prefab;
            _isWall = isWall;
            return Upgrade(owner, links, out city);
        }

        public override bool Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out AEdifice city)
        {
            _owner = owner;
            if (base.Upgrade(owner, links, out city))
                return true;

            _owner = PlayerType.None;
            return false;
        }

        public override void AddRoad(LinkType type, PlayerType owner) => _graphic.AddRoad(type, owner);

        public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
    }
}
