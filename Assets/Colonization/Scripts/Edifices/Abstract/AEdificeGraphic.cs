using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeGraphic : MonoBehaviour
    {
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        public virtual void Init(Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneObjects.Get<Players>()[owner].MaterialLit, _idMaterial);
        }

        public virtual void AddRoad(Id<LinkId> linkId, Id<PlayerId> owner) { }
    }
}
