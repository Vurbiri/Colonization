//Assets\Colonization\Scripts\Edifices\Abstract\AEdificeGraphic.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeGraphic : MonoBehaviour
    {
        public abstract void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links);

        public virtual void AddRoad(Id<LinkId> linkId) { }

    }
}
