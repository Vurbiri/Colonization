using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hexagon : MonoBehaviour, ISelectable
{
    [SerializeField, GetComponentInChildren] private TMP_Text _idText;
    [Space]
    [SerializeField, GetComponentInChildren] private MeshFilter _meshFilter;
    [SerializeField, GetComponentInChildren] private MeshRenderer _meshRenderer;
    [Space]
    [SerializeField, GetComponent] private Collider _collider;

    public Key Key => _key;
    public int Id => _id;
    public bool IsGate => _isGate;
    public bool IsWater => _isWater;
    public CurrencyType Currency => _surface.GetCurrency();

    #region private
    private int _id = -1;
    private Key _key;
    private SurfaceScriptable _surface;
    bool _isGate, _isWater;

    private readonly HashSet<Crossroad> _crossroads = new(CONST.COUNT_SIDES);
    private readonly HashSet<Hexagon> _neighbors = new(CONST.COUNT_SIDES);

    private const string NAME = "Hexagon_";
    #endregion

    public void Initialize(HexagonData data, float waterLevel)
    {
        (_key, _id, _surface) = data.GetValues();
        _idText.text = _id.ToString();

        _isGate = _surface.IsGate;
        _isWater = _surface.IsWater;

        _collider.enabled = !_isWater;

        EventBus.Instance.EventHexagonIdShow += _idText.gameObject.SetActive;

        name = NAME + _id + "__" + _key.ToString();

        if (_surface.Prefab == null)
            return;

        ASurface graphics = Instantiate(_surface.Prefab, transform);
        graphics.Initialize(_isWater ? waterLevel : 0f);

    }

    public void NeighborAdd(Hexagon neighbor) => _neighbors.Add(neighbor);

    public void CrossroadAdd(Crossroad crossroad) => _crossroads.Add(crossroad);
    public void CrossroadRemove(Crossroad crossroad) => _crossroads.Remove(crossroad);

    public bool IntersectWith(Hexagon other, out HashSet<Crossroad> set)
    {
        set = new(_crossroads);
        set.IntersectWith(other._crossroads);
        return set.Count == 2;
    }

    public bool IsWaterBusy()
    {
        if(!_isWater) return false;

        foreach(var crossroad in _crossroads)
            if(crossroad.Owner != PlayerType.None)
                return true;

        return false;
    }

    public void Select()
    {
        Debug.Log($"{gameObject.name}, water: {IsWater}, gate {IsGate}\n");
    }

}
