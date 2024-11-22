//Assets\Colonization\Scripts\Edifices\Water\PortTwoGraphic.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortTwoGraphic : AEdificeGraphicReColor
    {
        private static readonly IdArray<LinkId, Quaternion> angles = new(
            new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) });

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            base.Init(playerId, links);

            transform.localRotation = angles[links.FirstNullIndex()];
        }
    }
}
