using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    sealed public class LighthouseOneGraphic : PortOneGraphic
    {
        [Space]
        [SerializeField] private Mesh _altMesh;

        public override WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            if (Chance.Rolling())
                GetComponent<MeshFilter>().sharedMesh = _altMesh;

            return base.Init(playerId, links, isSFX);
        }
    }
}
