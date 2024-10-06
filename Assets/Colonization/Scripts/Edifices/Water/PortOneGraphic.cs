using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortOneGraphic : AEdificeGraphic
    {
        private static readonly IdArray<LinkId, Quaternion> anglesMirror = new(
            new Quaternion[] { Quaternion.Euler(0f, 300, 0f), Quaternion.Euler(0f, 60f, 0f), Quaternion.Euler(0f, 180f, 0f) });

        public override void Initialize(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            base.Initialize(playerId, links);

            foreach (var link in links)
            {
                if (!link.IsWater)
                {
                    transform.localRotation = anglesMirror[link.Id];
                    break;
                }
            }
        }
    }
}
