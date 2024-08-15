using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeGraphic : MonoBehaviour
    {
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        public virtual void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(Players.Instance[owner].MaterialLit, _idMaterial);
        }

        public virtual void AddRoad(LinkType type, PlayerType owner) { }
    }
}
