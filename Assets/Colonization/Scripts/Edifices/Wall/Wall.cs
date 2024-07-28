using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Currencies _cost;
        [Space, GetComponentInChildren]
        [SerializeField] private WallGraphic _graphic;

        public Currencies Cost => _cost;

        public virtual void Build(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            gameObject.SetActive(true);
            _graphic.Initialize(owner, links);
        }

        public void AddRoad(LinkType type, PlayerType owner) => _graphic.AddRoad(type, owner);

        public void Hide() => gameObject.SetActive(false);
    }
}
