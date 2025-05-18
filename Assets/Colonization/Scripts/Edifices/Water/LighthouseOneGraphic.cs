//Assets\Colonization\Scripts\Edifices\Water\LighthouseOneGraphic.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    sealed public class LighthouseOneGraphic : PortOneGraphic
    {
        [Space]
        [SerializeField] private Mesh _altMesh;

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            if (Chance.Rolling())
                GetComponent<MeshFilter>().sharedMesh = _altMesh;

            base.Init(playerId, links);
        }
    }
}
