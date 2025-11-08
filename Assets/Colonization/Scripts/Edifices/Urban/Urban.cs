using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public class Urban : AEdifice
    {
        [Space]
        [SerializeField] private Wall _wall;

        public override WaitSignal Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice, bool isSFX)
        {
            if (edifice.Id == EdificeId.Empty)
            {
                foreach (var link in links)
                {
                    if (link.Owner == playerId)
                    {
                        _graphic.transform.localRotation = CROSS.LINK_ROTATIONS[link.Id.Value];
                        break;
                    }
                }
            }

            if (isWall)
                _wall = edifice.WallTransfer(transform);

            return base.Init(playerId, isWall, links, edifice, isSFX);
        }

        public override Wall WallTransfer(Transform newParent)
        {
            _wall.transform.SetParent(newParent, false);
            return _wall;
        }

        public override ReturnSignal WallBuild(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            _wall = Instantiate(_wall, transform);
            return _wall.Init(playerId, links, isSFX);
        }

        public override void AddRoad(Id<LinkId> linkId, bool isWall)
        {
            if (isWall && _wall != null)
                _wall.AddRoad(linkId);
        }
        public override void RemoveRoad(Id<LinkId> linkId, bool isWall)
        {
            if (isWall && _wall != null)
                _wall.RemoveRoad(linkId);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (UnityEngine.Application.isPlaying) return;

            _settings.id = Mathf.Clamp(_settings.id, EdificeId.Camp, EdificeId.City);
            _settings.nextId = _settings.id + 1;

            _settings.groupId = EdificeGroupId.Colony;
            _settings.nextGroupId = EdificeGroupId.Colony;

            _settings.wallDefense = _settings.id - EdificeId.Camp;
            _settings.profit = _settings.wallDefense + 1;

            _settings.isBuildWall = _settings.wallDefense > 0;

            if (_settings.id == EdificeId.City)
            {
                _settings.nextId = EdificeId.None;
                _settings.nextGroupId = EdificeGroupId.None;
            }

            _settings.isUpgrade = _settings.nextId != EdificeId.None;

            base.OnValidate();
        }
#endif
    }
}
