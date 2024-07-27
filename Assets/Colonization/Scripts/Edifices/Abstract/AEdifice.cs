using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdifice : MonoBehaviour, IValueTypeEnum<EdificeType>
    {
        [SerializeField] private EdificeType _type;
        [SerializeField, Hide] private EdificeGroup _group;
        [SerializeField, Range(0, 3)] private int _level;
        [SerializeField, Hide] protected bool _isUpgrade;
        [SerializeField, Hide] protected bool _isBuildWall;
        [SerializeField] protected Currencies _cost;
        [Space, GetComponentInChildren]
        [SerializeField] protected AEdificeGraphic _graphic;
        [Space]
        [SerializeField] protected AEdifice _prefabUpgrade;
        [SerializeField, Hide] protected EdificeType _typeNext;
        [SerializeField, Hide] protected EdificeGroup _groupNext;
        [SerializeField, Range(1f, 5f)] private float _radiusCollider = 1.75f;

        public EdificeType Type => _type;
        public EdificeGroup Group => _group;
        public EdificeType TypeNext => _typeNext;
        public EdificeGroup GroupNext => _groupNext;
        public PlayerType Owner => _owner;
        public Currencies Cost => _cost;
        public int Level => _level;
        public bool IsBuildWall => _isBuildWall;
        public bool IsWall => _isWall;
        public float Radius => _radiusCollider;

        protected PlayerType _owner = PlayerType.None;
        protected bool _isWall = false;

        //TEST
        public void SetCost()
        {
            Debug.Log("TEST");
            _cost.Rand(_level);
        }

        public virtual void Initialize()
        {
            _owner = PlayerType.None;

            _graphic.Initialize();
        }

        public virtual void Setup(AEdifice edifice)
        {
            _owner = edifice._owner;
        }

        public virtual bool Build(AEdifice prefab, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out AEdifice edifice)
        {
            edifice = this;
            return false;
        }

        public virtual bool Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out AEdifice edifice)
        {
            if (_isUpgrade && _owner == owner)
            {
                edifice = Instantiate(_prefabUpgrade, transform.parent);
                edifice.Setup(this);
                edifice.EdificeUpgrade(_owner, links);

                Destroy(gameObject);
                return true;
            }

            edifice = this;
            return false;
        }

        public virtual bool CanBuyUpgrade(PlayerType owner, Currencies cash) => _isUpgrade && _owner == owner && _prefabUpgrade._cost <= cash;

        public virtual bool CanBuyWall(PlayerType owner, Currencies cash) => _isBuildWall;

        public virtual void AddLink(LinkType type) { }

        public virtual void AddRoad(LinkType type, PlayerType owner) { }

        public virtual void Show(bool isShow) { }

        protected virtual void EdificeUpgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links) => _graphic.Upgrade(_owner, links);

    }
}
