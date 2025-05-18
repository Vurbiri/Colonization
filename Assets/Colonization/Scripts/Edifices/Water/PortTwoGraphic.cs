//Assets\Colonization\Scripts\Edifices\Water\PortTwoGraphic.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    sealed public class PortTwoGraphic : AEdificeGraphicReColor
    {
        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            base.Init(playerId, links);

            Quaternion[] angles = { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) };
            transform.localRotation = angles[links.FirstNullIndex()];
        }
    }
}
