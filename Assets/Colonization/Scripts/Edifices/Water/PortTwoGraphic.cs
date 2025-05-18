//Assets\Colonization\Scripts\Edifices\Water\PortTwoGraphic.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    sealed public class PortTwoGraphic : AEdificeGraphicReColor
    {
        private static readonly Quaternion[] s_angles = { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) };

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            base.Init(playerId, links);
            transform.localRotation = s_angles[links.FirstNullIndex()];
        }
    }
}
