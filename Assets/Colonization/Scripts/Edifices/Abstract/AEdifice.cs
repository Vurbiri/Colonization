using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdifice : MonoBehaviour, IValueTypeEnum<EdificeType>, ISerializationCallbackReceiver
    {
        [SerializeField] protected EdificeType _type;
        [SerializeField, Hide] private EdificeGroup _group;
        [SerializeField, Range(0, 3)] private int _profit;
        [SerializeField, Hide] protected bool _isUpgrade;
        [SerializeField, Hide] protected bool _isBuildWall;
        [SerializeField] protected Currencies _cost;
        [Space]
        [SerializeField] protected AEdifice _prefabUpgrade;
        [SerializeField, Hide] protected EdificeType _typeNext;
        [SerializeField, Hide] protected EdificeGroup _groupNext;
        [SerializeField, Range(1f, 5f)] private float _radiusCollider = 1.75f;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;

        public EdificeType Type => _type;
        public EdificeGroup Group => _group;
        public EdificeType TypeNext => _typeNext;
        public EdificeGroup GroupNext => _groupNext;
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
        public void OnBeforeSerialize()
        {
            _group = _type.ToGroup();
            _isBuildWall = _group == EdificeGroup.Urban && _type != EdificeType.Camp;
            
            if(_isUpgrade = _prefabUpgrade != null)
            {
                _typeNext = _prefabUpgrade._type;
                _groupNext = _prefabUpgrade._group;
            }
            else
            {
                _typeNext = EdificeType.None;
                _groupNext = EdificeGroup.None;
            }
        }

        public void OnAfterDeserialize() { }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            _graphic = GetComponentInChildren<AEdificeGraphic>();
        }
#endif

    }
}
