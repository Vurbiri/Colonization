using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdifice : MonoBehaviour, IValueId<IdEdifice>
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
        [SerializeField] protected Id<IdEdifice> _id;
        [SerializeField, Hide] private int _idGroup;
        [SerializeField, Range(0, 3)] private int _profit;
        [SerializeField, Hide] protected bool _isUpgrade;
        [SerializeField, Hide] protected bool _isBuildWall;
        [SerializeField] protected Currencies _cost;
        [Space]
        [SerializeField] protected AEdifice _prefabUpgrade;
        [SerializeField, Hide] protected int _idNext;
        [SerializeField, Hide] protected int _idGroupNext;
        [SerializeField, Range(1f, 5f)] private float _radiusCollider = 1.75f;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;

        public Id<IdEdifice> Id => _id;
        public int IdGroup => _idGroup;
        public int IdNext => _idNext;
        public int IdGroupNext => _idGroupNext;
        public PlayerType Owner => _owner;
        public bool IsUpgrade => _isUpgrade;
        public bool IsOccupied => _owner != PlayerType.None;
        public Currencies Cost => _cost;
        public int Profit => _profit;
        public bool IsBuildWall => _isBuildWall;
        public bool IsWall => _isWall;
        public float Radius => _radiusCollider;

        protected PlayerType _owner = PlayerType.None;
        protected bool _isWall = false;

        //TEST
        public void SetCost()
        {
            Debug.Log("TEST (AEdifice)");
            _cost.Rand(_profit);
        }

        public virtual void Setup(AEdifice edifice, EnumHashSet<LinkType, CrossroadLink> links)
        {
            _owner = edifice._owner;
            _isWall = edifice._isWall;

            _graphic.transform.localRotation = edifice._graphic.transform.localRotation;
            _graphic.Initialize(_owner, links);
        }

        public virtual bool Build(AEdifice prefab, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, bool isWall, out AEdifice edifice)
        {
            edifice = this;
            return false;
        }

        public virtual bool Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out AEdifice edifice)
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

        public virtual bool WallBuild(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out Currencies cost)
        {
            cost = null;
            return _isBuildWall;
        }

        public virtual bool CanUpgradeBuy(Currencies cash) => _isUpgrade && _prefabUpgrade._cost <= cash;

        public virtual bool CanWallBuild(PlayerType owner) => _isBuildWall && _owner == owner;
        public virtual bool CanWallBuy(Currencies cash) => _isBuildWall;

        public virtual void AddRoad(LinkType type, PlayerType owner) { }

        public virtual void Show(bool isShow) { }


        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _idGroup = IdEdifice.ToGroup(_id);
            _isBuildWall = _idGroup == IdEdificeGroup.Urban && _id != IdEdifice.Camp;
            
            if(_isUpgrade = _prefabUpgrade != null)
            {
                _idNext = _prefabUpgrade._id;
                _idGroupNext = _prefabUpgrade._idGroup;
            }
            else
            {
                _idNext = IdEdifice.None;
                _idGroupNext = IdEdificeGroup.None;
            }
        }

        public void OnAfterDeserialize() { }
#endif
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            _graphic = GetComponentInChildren<AEdificeGraphic>();
        }
#endif

    }
}
