using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WallGraphic : AEdificeSidesGraphic<WallGate>
    {
        public override void Initialize(PlayerType owner, IdHashSet<LinkId, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(Players.Instance[owner].MaterialUnlit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.Owner != PlayerType.None);
        }

        public override void AddRoad(Id<LinkId> linkId, PlayerType owner) => _graphicSides[linkId].Open(true);
    }
}
