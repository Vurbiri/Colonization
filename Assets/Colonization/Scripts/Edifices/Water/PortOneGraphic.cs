using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortOneGraphic : AEdificeGraphic
    {
        private readonly Quaternion[] _anglesMirror = { Quaternion.Euler(0f, 300, 0f), Quaternion.Euler(0f, 60f, 0f), Quaternion.Euler(0f, 180f, 0f) };

        public override WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            foreach (var link in links)
            {
                if (!link.IsWater)
                {
                    transform.localRotation = _anglesMirror[link.Id.Value];
                    break;
                }
            }

            return base.Init(playerId, links, isSFX);
        }
    }
}
