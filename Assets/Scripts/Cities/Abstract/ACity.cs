using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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

    public virtual void RoadBuilt(LinkType type, int countFreeLink) => _graphic.RoadBuilt(type, countFreeLink);

    public virtual bool Upgrade(PlayerType owner, IEnumerable<LinkType> linkTypes, out ACity city)
    {
        Transform thisTransform = transform;
        city = Instantiate(_prefabNextUpgrade, thisTransform.position, Quaternion.identity, thisTransform.parent);
        city.Upgrade(owner, this);

        Destroy(gameObject);

        return true;
    }
    
    protected virtual void Upgrade(PlayerType owner, ACity city)
    {
        _owner = owner;
        _isGate = city._isGate;
        _waterCount = city._waterCount;
    }

    public virtual void Show(bool isShow) {}

    public int CompareTo(ACity other) => Type.CompareTo(other.Type);
}
