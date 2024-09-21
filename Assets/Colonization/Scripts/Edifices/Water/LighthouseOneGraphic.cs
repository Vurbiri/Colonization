using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class LighthouseOneGraphic : PortOneGraphic
    {
        [Space]
        [SerializeField] private Mesh _altMesh;

        private static Chance chanceAltMesh = new(50);

        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            if(chanceAltMesh.Roll)
                GetComponent<MeshFilter>().sharedMesh = _altMesh;
            
            base.Initialize(owner, links);

        }
    }
}
