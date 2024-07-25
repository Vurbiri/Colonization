using UnityEngine;

public abstract class AEdifice : MonoBehaviour, IValueTypeEnum<EdificeType>
{
    [SerializeField] private EdificeType _type;
    [SerializeField, Range(0, 3)] private int _level;
    [SerializeField] protected bool _isUpgrade;
    [SerializeField] protected Currencies _cost;
    [Space, GetComponentInChildren]
    [SerializeField] protected AEdificeGraphic _graphic;
    [Space]
    [SerializeField] protected AEdifice _prefabUpgrade;
    [Space]
    [SerializeField] private float _radiusCollider = 1.75f;

    public EdificeType Type => _type;
    public EdificeGroup Group => _group;
    public EdificeType TypeNext => _typeNext;
    public PlayerType Owner => _owner;
    public Currencies Cost => _cost;
    public int Level => _level;
    public bool IsUpgrade => _isUpgrade;
    public float Radius => _radiusCollider;

    protected PlayerType _owner = PlayerType.None;
    protected EdificeGroup _group;
    protected EdificeType _typeNext;

    //TEST
    public void SetCost()
    {
        Debug.Log("TEST");
        _cost.Rand(_level);
    }

    public virtual void Initialize()
    {
        _owner = PlayerType.None;
        _group = _type.ToGroup();

        _graphic.Initialize();
    }

    public virtual void Setup(AEdifice edifice)
    {
        _owner = edifice._owner;
        _group = _type.ToGroup();
        _typeNext = _isUpgrade && _prefabUpgrade != null ? _prefabUpgrade.Type : EdificeType.None;
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
            edifice._graphic.Upgrade(_owner, links);

            Destroy(gameObject);
            return true;
        }

        edifice = this;
        return false;
    }

    public virtual bool CanBuyUpgrade(PlayerType owner, Currencies cash) => _isUpgrade && _owner == owner && _prefabUpgrade._cost <= cash;

    public virtual void AddLink(LinkType type) { }

    public virtual void AddRoad(LinkType type, PlayerType owner) { }

    public virtual void Show(bool isShow) { }

    

}
