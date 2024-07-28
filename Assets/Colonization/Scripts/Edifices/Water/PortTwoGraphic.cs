using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PortTwoGraphic : AEdificeGraphic
    {
        private static readonly EnumArray<LinkType, Quaternion> angles = new(
            new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) });

        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            base.Initialize(owner, links);

            transform.localRotation = angles[links.FirstEmptyIndex()];
        }
    }
}
