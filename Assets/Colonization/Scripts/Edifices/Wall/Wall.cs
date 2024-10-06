using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Currencies _cost;
        [Space]
        [SerializeField] private WallGraphic _graphic;

        public Currencies Cost => _cost;

        public void Initialize(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            gameObject.SetActive(true);
            _graphic.Initialize(playerId, links);
        }

        public void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) => _graphic.AddRoad(linkId, playerId);


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_graphic == null)
                _graphic = GetComponentInChildren<WallGraphic>();
        }
#endif
    }
}
