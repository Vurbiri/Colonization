//Assets\Colonization\Scripts\Edifices\Urban\Urban.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Urban : AEdifice
    {
        [Space]
        [SerializeField] private Wall _wall;

        public override AEdifice Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice)
        {

            base.Init(playerId, isWall, links, edifice);

            if (edifice.Id == EdificeId.Signpost)
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

            return this;
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
