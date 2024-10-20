using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdifice : MonoBehaviour, IValueId<EdificeId>
    {
        [SerializeField] protected Id<EdificeId> _id;
        [SerializeField, Hide] private int _groupId;
        [SerializeField, Range(0, 3)] private int _profit;
        [SerializeField, Hide] protected bool _isUpgrade;
        [SerializeField, Hide] protected bool _isBuildWall;
        [Space]
        [SerializeField] protected AEdifice _prefabUpgrade;
        [SerializeField, Hide] protected int _nextId;
        [SerializeField, Hide] protected int _nextGroupId;
        [SerializeField, Range(1f, 5f)] private float _radiusCollider = 1.75f;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;

        protected Id<PlayerId> _owner = PlayerId.None;
        protected bool _isWall = false;

        public Id<EdificeId> Id => _id;
        public int GroupId => _groupId;
        public int NextId => _nextId;
        public int NextGroupId => _nextGroupId;
        public Id<PlayerId> Owner => _owner;
        public bool IsUpgrade => _isUpgrade;
        public bool IsOccupied => _owner != PlayerId.None;
        public bool IsUrban => _owner != PlayerId.None && _groupId == EdificeGroupId.Urban;
        public int Profit => _profit;
        public bool IsWall => _isWall;
        public float Radius => _radiusCollider;

        public virtual void Setup(AEdifice edifice, IdHashSet<LinkId, CrossroadLink> links)
        {
            _owner = edifice._owner;
            _isWall = edifice._isWall;

            _graphic.transform.localRotation = edifice._graphic.transform.localRotation;
            _graphic.Init(_owner, links);
        }

        public virtual bool Build(AEdifice prefab, Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links, bool isWall, out AEdifice edifice)
        {
            edifice = this;
            return false;
        }

        public virtual bool Upgrade(Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links, out AEdifice edifice)
        {
            if (_isUpgrade && _owner == owner)
            {
                edifice = Instantiate(_prefabUpgrade, transform.parent);
                edifice.Setup(this, links);

                Destroy(gameObject);
                return true;
            }

            edifice = this;
            return false;
        }

        public bool CanHiringWarriors(Id<PlayerId> owner) => _owner == owner && _groupId == EdificeGroupId.Port;

        public bool CanWallBuild(Id<PlayerId> owner) => _isBuildWall && _owner == owner;

        public virtual bool WallBuild(Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links) => _isBuildWall;

        public virtual void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) { }

        public virtual void Show(bool isShow) { }


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            _groupId = EdificeId.ToGroup(_id.ToInt);
            _isBuildWall = _groupId == EdificeGroupId.Urban && _id != EdificeId.Camp;

            if (_isUpgrade = _prefabUpgrade != null)
            {
                _nextId = _prefabUpgrade._id.ToInt;
                _nextGroupId = _prefabUpgrade._groupId;
            }
            else
            {
                _nextId = EdificeId.None;
                _nextGroupId = EdificeGroupId.None;
            }

            _graphic = GetComponentInChildren<AEdificeGraphic>();
        }
#endif
    }
}
