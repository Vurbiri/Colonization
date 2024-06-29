using UnityEngine;

public class City : MonoBehaviour, IValueTypeEnum<CityType>
{
    [SerializeField] private CityType _type;
    [SerializeField] private bool _isUpgrade = true;
    [SerializeField] private Currencies _cost;
    [Space, GetComponentInChildren]
    [SerializeField] protected CityGraphic _graphic;
    [Space]
    [SerializeField] protected City _prefabUpgrade;
    [Space]
    [SerializeField] private float _radiusCollider = 1.75f;

    public CityType Type => _type;
    public CityType TypeNext => _prefabUpgrade._type;
    public PlayerType Owner => _owner;
    public bool IsUpgrade => _isUpgrade;
    public float Radius => _radiusCollider;

    protected PlayerType _owner;

    public virtual void Initialize()
    {
        _owner = PlayerType.None;

        _graphic.Initialize();
    }


    public virtual void AddLink(LinkType type) => _graphic.AddLink(type);

    public virtual void AddRoad(LinkType type, PlayerType owner) => _graphic.RoadBuilt(type, owner);

    public virtual bool Build(CityType type, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out City city)
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
            city._graphic.Upgrade(links);

            Destroy(gameObject);
            return true;
        }

        city = this;
        return false;
    }
    
    protected virtual void CopyingData(City city)
    {
        _owner = city._owner;
    }

    public virtual void Show(bool isShow) {}

}
