using UnityEngine;

public class City : MonoBehaviour, IValueTypeEnum<CityType>
{
    [SerializeField] private CityType _type;
    [SerializeField, Range(0, 3)] private int _level;
    [SerializeField] protected bool _isUpgrade = true;
    [SerializeField] protected Currencies _cost;
    [Space, GetComponentInChildren]
    [SerializeField] protected ACityGraphic _graphic;
    [Space]
    [SerializeField] protected City _prefabUpgrade;
    [Space]
    [SerializeField] private float _radiusCollider = 1.75f;

    public CityType Type => _type;
    public CityGroup Group => _group;
    public CityType TypeNext => _prefabUpgrade._type;
    public PlayerType Owner => _owner;
    public Currencies Cost => _cost;
    public int Level => _level;
    public bool IsUpgrade => _isUpgrade;
    public float Radius => _radiusCollider;

    protected PlayerType _owner = PlayerType.None;
    protected CityGroup _group;

    //TEST
    public void SetCost() => _cost.Rand(_level);

    public virtual void Initialize()
    {
        _owner = PlayerType.None;
        _group = _type.ToGroup();

        _graphic.Initialize();
    }

    public virtual void AddLink(LinkType type) => _graphic.AddLink(type);

    public virtual void AddRoad(LinkType type, PlayerType owner) => _graphic.RoadBuilt(type, owner);

    public virtual bool Build(City prefab, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out City city)
    {
        city = this;
        return false;
    }

    public virtual bool Upgrade(EnumHashSet<LinkType, CrossroadLink> links, out City city)
    {
        if (_isUpgrade)
        {
            city = Instantiate(_prefabUpgrade, transform.parent);
            city.CopyingData(this);
            city._graphic.Upgrade(_owner, links);

            Destroy(gameObject);
            return true;
        }

        city = this;
        return false;
    }
    
    protected virtual void CopyingData(City city)
    {
        _owner = city._owner;

        _group = _type.ToGroup();
    }

    public bool CanBuyUpgrade(PlayerType owner, Currencies cash) => _isUpgrade && _owner == owner && _prefabUpgrade._cost <= cash;
    
    public virtual void Show(bool isShow) {}
}
