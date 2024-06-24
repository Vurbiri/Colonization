using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACity : MonoBehaviour, IComparable<ACity>, ITypeValueEnum<CityType>
{
    [SerializeField] private float _radiusCollider = 1.75f;
    [Space]
    [SerializeField] protected ACity _prefabNextUpgrade;

    public abstract CityType Type { get; }
    public abstract PlayerType Owner { get; }
    public bool IsUpgrade => _isUpgrade;
    public float Radius => _radiusCollider;

    protected bool _isGate = false, _isUpgrade = false;
    protected int _waterCount = 0;

    public virtual bool Setup(CityDirection direction, ICollection<LinkType> linkTypes)
    {

        if (direction == CityDirection.None) 
            return false;

        if (direction == CityDirection.Up)
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        else
            transform.rotation *= Quaternion.identity;

        return true;
    }

    public bool SetHexagon(Hexagon hex, int count)
    {
        _isGate = _isGate || hex.IsGate;

        if (hex.IsWater)
            _waterCount++;

        return !(_isUpgrade =_waterCount < count);
    }

    public bool Upgrade(ICollection<LinkType> linkTypes, out ACity city)
    {
        if(!_isUpgrade)
        {
            city = this;
            return false;
        }

        city = Instantiate(_prefabNextUpgrade, Vector3.zero, transform.localRotation, transform.parent);
        city.Upgrade(this);

        Destroy(gameObject);

        return true;
    }

    public abstract void RoadBuilt(LinkType type, int countFreeLink);

    protected virtual void Upgrade(ACity city)
    {
        _isGate = city._isGate;
        _waterCount = city._waterCount;
    }

    public int CompareTo(ACity other) => Type.CompareTo(other.Type);
}
