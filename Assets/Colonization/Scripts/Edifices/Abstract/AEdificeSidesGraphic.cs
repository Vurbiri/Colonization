using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdificeSidesGraphic : AEdificeGraphic
    {
        [Space]
        [SerializeField] protected EnumHashSet<LinkType, AEdificeSide> _graphicSides;

        public override void Initialize()
        {
            _players = Players.Instance;

            foreach (var side in _graphicSides)
                side.SetActive(false);
        }

        public override void Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            Initialize();

            LinkType type;
            foreach (var link in links)
            {
                type = link.Type; owner = link.Owner;
                AddLink(type);
                if (owner != PlayerType.None)
                    RoadBuilt(type, owner);
            }
        }

        public override void AddLink(LinkType type) => _graphicSides[type].SetActive(true);

        public override void RoadBuilt(LinkType type, PlayerType owner) => _graphicSides[type].SetMaterial(_players[owner].Material);
    }
}
