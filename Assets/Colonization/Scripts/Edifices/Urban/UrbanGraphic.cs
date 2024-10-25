using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class UrbanGraphic : AEdificeGraphicReColor
    {
        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneData.Get<PlayersVisual>()[playerId].materialLit, _idMaterial);

            foreach (var link in links)
            {
                if (link.Owner == playerId)
                {
                    transform.localRotation = CONST.LINK_ROTATIONS[link.Id];
                    return;
                }
            }
        }
    }
}
