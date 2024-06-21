using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hexagon : MonoBehaviour, ISelectable
{
    [SerializeField] private TMP_Text _idText;
    
    public Key Key => _key;
    public int Id => _id;
    public bool IsGate => _surface == SurfaceType.Gate;
    public bool IsWater => _surface == SurfaceType.Water;
    public HashSet<Crossroad> Crossroads => _crossroads;
    public HashSet<Hexagon> Neighbors => _neighbors;

    #region private
    private int _id = -1;
    private Key _key;
    private SurfaceType _surface;

    private readonly HashSet<Crossroad> _crossroads = new(CONST.COUNT_SIDES);
    private readonly HashSet<Hexagon> _neighbors = new(CONST.COUNT_SIDES);

    private const string NAME = "Hexagon_";
    #endregion

    public void Initialize(Key key, SurfaceScriptable surface, float waterLevel, int id)
    {
        _key = key;
        _id = id;
        _idText.text = _id.ToString();

        _surface = surface.Type;

        name = NAME + _id + "__" + key.ToString();

        if (IsWater)
        {
            transform.localPosition += new Vector3(0f, waterLevel, 0f);
            GetComponent<Collider>().enabled = false;
        }
    }

    public void Select()
    {

        Debug.Log($"{gameObject.name}, water: {IsWater}, gate {IsGate}\n");
    }

    

    //public void SetNewPosition(Vector2Int index)
    //{
    //    _key = index;
    //    _thisTransform.localPosition = new(_offset.x * 0.5f * index.x, 0, _offset.y * index.y);
    //}
}
