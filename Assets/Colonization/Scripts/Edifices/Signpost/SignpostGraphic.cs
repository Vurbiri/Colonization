using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class SignpostGraphic : AEdificeGraphic
    {
        [SerializeField] private IdHashSet<LinkId, SignpostSide> _graphicSides;

        private int _countRoads = 0;

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            foreach (var side in _graphicSides)
                side.SetActive(links[side.Id.Value] != null);

            SceneServices.Get<GameplayEventBus>().EventCrossroadMarkShow += Show;
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId)
        {
            _countRoads++;
            gameObject.SetActive(false);
        }

        private void Show(bool isShow) 
        {
            gameObject.SetActive(isShow && _countRoads == 0);
        }

        private void OnDestroy()
        {
            SceneServices.Get<GameplayEventBus>().EventCrossroadMarkShow -= Show;
        }
    }
}
