using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortOneGraphic : AEdificeGraphic
    {
        public override WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            foreach (var link in links)
            {
                if (link.IsNotShore)
                {
                    transform.localRotation = CROSS.LINK_MIRROR[link.Id.Value];
                    break;
                }
            }

            return base.Init(playerId, links, isSFX);
        }
    }
}
