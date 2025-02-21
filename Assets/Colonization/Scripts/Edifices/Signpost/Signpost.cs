//Assets\Colonization\Scripts\Edifices\Signpost\Signpost.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public class Signpost : AEdificeSelectable
    {
        [Space]
        [SerializeField] protected GameObject _graphicObject;
        [SerializeField] private IdArray<LinkId, GameObject> _sides;

        private int _countRoads = 0;

        public override AEdifice Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice)
        {
            GameObject side;
            for (int i = 0; i < LinkId.Count; i++)
            {
                side = _sides[i];
                if (links[i] == null)
                {
                    Destroy(side);
                    continue;
                }

                side.transform.localRotation = CONST.LINK_ROTATIONS[i];
                side.SetActive(true);
            }

            SceneServices.Get<GameplayEventBus>().EventCrossroadMarkShow += Show;
            return this;
        }

        public override void AddRoad(Id<LinkId> linkId, bool isWall)
        {
            _countRoads++;
            _graphicObject.SetActive(false);
        }

        private void Show(bool isShow)
        {
            _graphicObject.SetActive(isShow && _countRoads == 0);
        }

        private void OnDestroy()
        {
            SceneServices.Get<GameplayEventBus>().EventCrossroadMarkShow -= Show;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _settings.id = EdificeId.Signpost;
            _settings.groupId = EdificeGroupId.None;
            _settings.nextId = EdificeId.Signpost;
            _settings.nextGroupId = EdificeGroupId.None;

            _settings.isBuildWall = false;
            _settings.isUpgrade = true;

            _settings.profit = 0;

            if (_collider == null)
                _collider = GetComponent<Collider>();
        }
#endif
    }
}
