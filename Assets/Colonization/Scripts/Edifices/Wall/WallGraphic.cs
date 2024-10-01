using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WallGraphic : AEdificeSidesGraphic<WallGate>
    {
        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(Players.Instance[owner].MaterialUnlit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Type].Open(link.Owner != PlayerType.None);
        }

        public override void AddRoad(LinkType type, PlayerType owner) => _graphicSides[type].Open(true);
    }
}
