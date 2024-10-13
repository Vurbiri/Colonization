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
        [SerializeField] protected PricesScriptable _prices;
        [Space]
        [SerializeField] protected AEdifice _prefabUpgrade;
        [SerializeField, Hide] protected int _nextId;
        [SerializeField, Hide] protected int _nextGroupId;
        [SerializeField, Range(1f, 5f)] private float _radiusCollider = 1.75f;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;

        protected ACurrencies _costUpgrade;
        protected ACurrencies _costWall;
        protected Id<PlayerId> _owner = PlayerId.None;
        protected bool _isWall = false;

        public Id<EdificeId> Id => _id;
        public int GroupId => _groupId;
        public int NextId => _nextId;
        public int NextGroupId => _nextGroupId;
        public Id<PlayerId> Owner => _owner;
        public bool IsUpgrade => _isUpgrade;
        public bool IsOccupied => _owner != PlayerId.None;
        public int Profit => _profit;
        public bool IsWall => _isWall;
        public float Radius => _radiusCollider;

        public virtual void Setup(AEdifice edifice, IdHashSet<LinkId, CrossroadLink> links)
        {
            _owner = edifice._owner;
            _isWall = edifice._isWall;

            SetCost();

            _graphic.transform.localRotation = edifice._graphic.transform.localRotation;
            _graphic.Init(_owner, links);
        }

        public virtual bool Build(AEdifice prefab, Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links, bool isWall, out AEdifice edifice)
        {
            edifice = this;
            return false;
        }

        public virtual bool Upgrade(Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links, out AEdifice edifice, out ACurrencies cost)
        {
            if (_isUpgrade && _owner == owner)
            {
                cost = _costUpgrade;
                edifice = Instantiate(_prefabUpgrade, transform.parent);
                edifice.Setup(this, links);

                Destroy(gameObject);
                return true;
            }

            cost = CurrenciesLite.Empty;
            edifice = this;
            return false;
        }

        public virtual bool WallBuild(Id<PlayerId> owner, IdHashSet<LinkId, CrossroadLink> links, out ACurrencies cost)
        {
            cost = null;
            return _isBuildWall;
        }

        public virtual bool CanUpgradeBuy(ACurrencies cash) => _isUpgrade && cash >= _costUpgrade;

        public virtual bool CanWallBuild(Id<PlayerId> owner) => _isBuildWall && _owner == owner;
        public virtual bool CanWallBuy(ACurrencies cash) => _isBuildWall;

        public virtual void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) { }

        public virtual void Show(bool isShow) { }

        public virtual void ClearResources()
        {
            _prices = null;
        }

        protected void SetCost()
        {
            _costUpgrade = _isUpgrade ? _prices.Edifices[_nextId] : CurrenciesLite.Empty;
            _costWall = _isBuildWall ? _prices.Wall : CurrenciesLite.Empty;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_prices == null)
                _prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();


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
