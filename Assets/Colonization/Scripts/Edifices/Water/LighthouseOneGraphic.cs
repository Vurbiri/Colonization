using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class LighthouseOneGraphic : PortOneGraphic
    {
        [Space]
        [SerializeField] private Mesh _altMesh;
        
        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            if(Chance.Rolling())
                GetComponent<MeshFilter>().sharedMesh = _altMesh;
            
            base.Initialize(owner, links);

        }
    }
}
