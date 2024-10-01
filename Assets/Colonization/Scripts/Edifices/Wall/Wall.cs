using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Currencies _cost;
        [Space]
        [SerializeField] private WallGraphic _graphic;

        public Currencies Cost => _cost;

        public void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            gameObject.SetActive(true);
            _graphic.Initialize(owner, links);
        }

        public void AddRoad(LinkType type, PlayerType owner) => _graphic.AddRoad(type, owner);


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_graphic == null)
                _graphic = GetComponentInChildren<WallGraphic>();
        }
#endif
    }
}
