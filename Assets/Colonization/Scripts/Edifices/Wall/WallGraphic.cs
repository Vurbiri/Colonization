using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WallGraphic : AEdificeGraphicReColor
    {
        [Space]
        [SerializeField] protected IdHashSet<LinkId, WallGate> _graphicSides;

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneData.Get<PlayersVisual>()[playerId].materialUnlit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.Owner != PlayerId.None);
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) => _graphicSides[linkId].Open(true);
    }
}
