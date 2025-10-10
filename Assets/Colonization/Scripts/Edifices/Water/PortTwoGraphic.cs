using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    sealed public class PortTwoGraphic : AEdificeGraphic
    {
        public override WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            transform.localRotation = CROSS.LINK_ROTATIONS[NullIndex(links)];
            return base.Init(playerId, links, isSFX);

            //Local
            [MethodImpl(256)]
            static int NullIndex(IReadOnlyList<CrossroadLink> links)
            {
                int index = -1;
                for (int i = links.Count - 1; i >= 0; i--)
                    if (links[i] == null)
                        index = i;
                return index;
            }
        }
    }
}
