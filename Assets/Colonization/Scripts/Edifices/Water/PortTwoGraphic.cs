using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortTwoGraphic : AEdificeGraphic
    {
        private static readonly IdArray<LinkId, Quaternion> angles = new(
            new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) });

        public override void Initialize(PlayerType owner, IdHashSet<LinkId, CrossroadLink> links)
        {
            base.Initialize(owner, links);

            transform.localRotation = angles[links.FirstEmptyIndex()];
        }
    }
}
