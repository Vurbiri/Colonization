using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hexagon : MonoBehaviour, ISelectable
{
    [SerializeField] private bool _showId = true;
    [SerializeField] private TMP_Text _idText;
    
    public Key Key => _key;
    public int Id => _id;
    public bool IsGate => _isGate;
    public bool IsWater => _isWater;

    #region private
    private int _id = -1;
    private Key _key;
    private Resource _surface;
    bool _isGate, _isWater;

    private readonly HashSet<Crossroad> _crossroads = new(CONST.COUNT_SIDES);
    private readonly HashSet<Hexagon> _neighbors = new(CONST.COUNT_SIDES);

    private const string NAME = "Hexagon_";
    #endregion

    public void Initialize(Key key, SurfaceScriptable surface, float waterLevel, int id)
    {
        _key = key;
        _id = id;
        _idText.text = _id.ToString();
        _idText.gameObject.SetActive(_showId);

        _surface = surface.Type;
        _isGate = _surface == Resource.Gate;
        _isWater = _surface == Resource.Water;

        name = NAME + _id + "__" + key.ToString();

        if (IsWater)
        {
            transform.localPosition += new Vector3(0f, waterLevel, 0f);
            GetComponent<Collider>().enabled = false;
        }
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
