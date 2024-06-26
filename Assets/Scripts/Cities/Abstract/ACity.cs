using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACity : MonoBehaviour, IComparable<ACity>, IValueTypeEnum<CityType>
{
    [GetComponentInChildren]
    [SerializeField] protected ACityGraphic _graphic;
    [Space]
    [SerializeField] protected ACity _prefabNextUpgrade;
    [Space]
    [SerializeField] private float _radiusCollider = 1.75f;

    public abstract CityType Type { get; }
    public virtual PlayerType Owner => _owner;
    public virtual bool IsUpgrade => true;
    public float Radius => _radiusCollider;

    protected bool _isGate = false;
    protected int _waterCount = 0;
    protected PlayerType _owner;

    public virtual void Initialize() => _graphic.Initialize();

    public virtual bool Setup() => _waterCount < Crossroad.COUNT;

    public void SetHexagon(Hexagon hex)
    {
        _isGate = _isGate || hex.IsGate;

        if (hex.IsWater)
            _waterCount++;
    }

    public virtual void AddLink(LinkType type) => _graphic.AddLink(type);

    public virtual void AddRoad(LinkType type) => _graphic.RoadBuilt(type);

    public virtual bool Upgrade(PlayerType owner, IEnumerable<CrossroadLink> links, out ACity city)
    {
        if(!IsUpgrade || _owner != owner)
        {
            city = this;
            return false;
        }
        
        city = Instantiate(_prefabNextUpgrade, transform.parent);
        city.Initialize();
        city.CopyingData(links, this);

        Destroy(gameObject);
        return true;
    }
    
    protected virtual void CopyingData(IEnumerable<CrossroadLink> links, ACity city)
    {
        _owner = city._owner;
        _isGate = city._isGate;
        _waterCount = city._waterCount;

        foreach (CrossroadLink link in links)
        {
            AddLink(link.Type);
            if(link.Owner != PlayerType.None)
                AddRoad(link.Type);
        }
    }

    public virtual void Show(bool isShow) {}

    public int CompareTo(ACity other) => Type.CompareTo(other.Type);
}
