using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Urban : AEdifice
    {
        [Space, GetComponentInChildren]
        [SerializeField] private Wall _wall;

        public override void Setup(AEdifice edifice, EnumHashSet<LinkType, CrossroadLink> links)
        {
            base.Setup(edifice, links);

            if (_isWall)
                _wall.Build(_owner, links);
            else
                _wall.Hide();

            _isBuildWall = _isBuildWall && !_isWall;
        }

        public override bool WallBuild(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out Currencies cost)
        {
            if (_isBuildWall && _owner == owner)
            {
                _wall.Build(_owner, links);
                _isWall = true;
                _isBuildWall = false;
                cost = _wall.Cost;
                return true;
            }

            cost = null;
            return false;
        }

        public override bool CanWallBuy(Currencies cash) => _isBuildWall && _wall.Cost <= cash;

        public override void AddRoad(LinkType type, PlayerType owner)
        {
            _graphic.AddRoad(type, owner);
            
            if(_isWall)
                _wall.AddRoad(type, owner);
        }
    }
}
