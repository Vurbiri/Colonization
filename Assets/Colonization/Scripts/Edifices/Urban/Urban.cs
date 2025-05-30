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
                        _graphic.transform.localRotation = CONST.LINK_ROTATIONS[link.Id.Value];
                        break;
                    }
                }
            }

            if (isWall)
                _wall = Instantiate(_wall, transform).Init(playerId, links);

            return base.Init(playerId, isWall, links, edifice, isSFX);
        }

        public override bool WallBuild(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            _wall = Instantiate(_wall, transform).Init(playerId, links);
            return true;
        }

        public override void AddRoad(Id<LinkId> linkId, bool isWall)
        {
            if (isWall && _wall != null)
                _wall.AddRoad(linkId);
        }
    }
}
