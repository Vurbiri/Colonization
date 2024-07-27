using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeGraphic : MonoBehaviour
    {
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        protected Players _players;

        public virtual void Initialize()
        {
            _players = Players.Instance;
        }

        public virtual void Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            _players = Players.Instance;
            GetComponent<MeshRenderer>().SetSharedMaterial(_players[owner].Material, _idMaterial);
        }

        public virtual void AddLink(LinkType type) { }

        public virtual void RoadBuilt(LinkType type, PlayerType owner) { }
    }
}
