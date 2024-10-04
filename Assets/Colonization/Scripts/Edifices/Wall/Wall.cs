using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Currencies _cost;
        [Space]
        [SerializeField] private WallGraphic _graphic;

        public Currencies Cost => _cost;

        public void Initialize(PlayerType owner, IdHashSet<LinkId, CrossroadLink> links)
        {
            gameObject.SetActive(true);
            _graphic.Initialize(owner, links);
        }

        public void AddRoad(Id<LinkId> linkId, PlayerType owner) => _graphic.AddRoad(linkId, owner);


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_graphic == null)
                _graphic = GetComponentInChildren<WallGraphic>();
        }
#endif
    }
}
