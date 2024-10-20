using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WallGraphic : AEdificeSidesGraphic<WallGate>
    {
        public override void Init(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneObjects.Get<Players>()[playerId].MaterialUnlit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.Owner != PlayerId.None);
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) => _graphicSides[linkId].Open(true);
    }
}
