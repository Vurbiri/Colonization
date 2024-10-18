using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Urban : AEdifice
    {
        [Space]
        [SerializeField] private Wall _wall;

        public override void Setup(AEdifice edifice, IdHashSet<LinkId, CrossroadLink> links)
        {
            base.Setup(edifice, links);

            if (_id == EdificeId.Camp)
            {
                foreach (var link in links)
                {
                    if (link.Owner == _owner)
                    {
                        _graphic.transform.localRotation = CONST.LINK_ROTATIONS[link.Id];
                        break;
                    }
                }
            }

            if (_isWall)
            {
                _wall = Instantiate(_wall, transform);
                _wall.Init(_owner, links);
            }

            _isBuildWall = _isBuildWall && !_isWall;
        }

        public override bool WallBuild(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            if (_isBuildWall && _owner == playerId)
            {
                _wall = Instantiate(_wall, transform);
                _wall.Init(_owner, links);
                _isWall = true;
                _isBuildWall = false;
                return true;
            }

            return false;
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> owner)
        {
            if(_isWall && _wall != null)
                _wall.AddRoad(linkId, owner);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(_isBuildWall && _wall == null)
                _wall = VurbiriEditor.Utility.FindAnyPrefab<Wall>();
        }
#endif
    }
}
