using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    sealed public class PortTwoGraphic : AEdificeGraphicReColor
    {
        private readonly Quaternion[] _angles = { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) };

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            transform.localRotation = _angles[links.FirstNullIndex()];
            base.Init(playerId, links);
        }
    }
}
