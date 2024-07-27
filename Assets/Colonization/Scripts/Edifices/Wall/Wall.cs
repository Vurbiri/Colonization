using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Currencies _cost;
        [Space, GetComponentInChildren]
        [SerializeField] private WallGraphic _graphic;

        public Currencies Cost => _cost;

        public void Initialize()
        {
            _graphic.Initialize();
        }

        public virtual bool Build(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {

            return false;
        }


        public void AddLink(LinkType type) => _graphic.AddLink(type);

        public void AddRoad(LinkType type, PlayerType owner) => _graphic.RoadBuilt(type, owner);
    }
}
