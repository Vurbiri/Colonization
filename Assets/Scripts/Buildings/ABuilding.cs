using UnityEngine;

public abstract class ABuilding : MonoBehaviour
{
    [SerializeField] private float _radiusCollider = 1.75f;
    [Space]
    [SerializeField] protected ABuilding _prefabNextUpgrade;
    
    public abstract BuildingType Type { get; }
    public bool IsUpgrade => _isUpgrade;

    public float Radius => _radiusCollider;

    protected bool _isGate = false, _isUpgrade = false;
    protected int _waterCount = 0;

    public virtual void Setup()
    {
        
    }

    public bool SetHexagon(Hexagon hex, int count)
    {
        _isGate = _isGate || hex.IsGate;

        if (hex.IsWater)
            _waterCount++;

        return !(_isUpgrade =_waterCount < count);
    }

    public bool Upgrade(out ABuilding building)
    {
        if(!_isUpgrade)
        {
            building = this;
            return false;
        }

        building = Instantiate(_prefabNextUpgrade, Vector3.zero, Quaternion.identity, transform.parent);
        building.Upgrade(this);

        Destroy(gameObject);

        return true;
    }

    protected virtual void Upgrade(ABuilding building)
    {
        _isGate = building._isGate;
        _waterCount = building._waterCount;
    }
}
