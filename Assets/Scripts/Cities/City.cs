using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, IValueTypeEnum<CityType>
{
    [SerializeField] private CityType _type;
    [SerializeField] private bool _isUpgrade = true;
    [Space, GetComponentInChildren]
    [SerializeField] protected CityGraphic _graphic;
    [Space]
    [SerializeField] protected City _prefabNextUpgrade;
    [Space]
    [SerializeField] private float _radiusCollider = 1.75f;

    public CityType Type => _type;
    public CityType TypeNext => _prefabNextUpgrade._type;
    public PlayerType Owner => _owner;
    public bool IsUpgrade => _isUpgrade;
    public float Radius => _radiusCollider;

    protected bool _isGate = false;
    private int _waterCount = 0;
    protected PlayerType _owner = PlayerType.None;

    public virtual void Initialize()
    {
        _graphic.Initialize();
    }

    public virtual bool Setup() => _waterCount < Crossroad.COUNT;

    public void SetHexagon(Hexagon hex)
    {
        _isGate = _isGate || hex.IsGate;

        if (hex.IsWater)
            _waterCount++;
    }

    public virtual void AddLink(LinkType type) => _graphic.AddLink(type);

    public virtual void AddRoad(LinkType type) => _graphic.RoadBuilt(type);

    public virtual bool Build(PlayerType owner, Material material, IEnumerable<CrossroadLink> links, out City city)
    {
        city = this;
        return false;
    }

    public virtual bool Upgrade(IEnumerable<CrossroadLink> links, out City city)
    {
        if (_isUpgrade)
        {
            city = Instantiate(_prefabNextUpgrade, transform.parent);
            city.CopyingData(links, this);

            Destroy(gameObject);
            return true;
        }

        city = this;
        return false;
    }
    
    protected virtual void CopyingData(IEnumerable<CrossroadLink> links, City city)
    {
        _owner = city._owner;
        _isGate = city._isGate;
        _waterCount = city._waterCount;

        _graphic.Upgrade(city._graphic);

        foreach (CrossroadLink link in links)
        {
            AddLink(link.Type);
            if(link.Owner != PlayerType.None)
                AddRoad(link.Type);
        }
    }

    public virtual void Show(bool isShow) {}

}
