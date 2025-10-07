using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    sealed public class PortTwoGraphic : AEdificeGraphic
    {
        public override WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            transform.localRotation = CROSS.LINK_ROTATIONS[links.FirstNullIndex()];
            return base.Init(playerId, links, isSFX);
        }
    }
}
