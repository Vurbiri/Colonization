//Assets\Colonization\Scripts\Edifices\Water\PortOneGraphic.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortOneGraphic : AEdificeGraphicReColor
    {

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            base.Init(playerId, links);

            Quaternion[] anglesMirror = { Quaternion.Euler(0f, 300, 0f), Quaternion.Euler(0f, 60f, 0f), Quaternion.Euler(0f, 180f, 0f) };
            foreach (var link in links)
            {
                if (!link.IsWater)
                {
                    transform.localRotation = anglesMirror[link.Id.Value];
                    break;
                }
            }
        }
    }
}
