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

    private readonly HashSet<Crossroad> _crossroads = new(CONST.HEX_SIDE);
    private readonly HashSet<Hexagon> _neighbors = new(CONST.HEX_SIDE);

#if UNITY_EDITOR
    private const string NAME = "Hexagon_";
#endif
    #endregion

    public void Initialize(Key key, SurfaceScriptable surface, int id)
    {
        _key = key;
        _id = id;
        _idText.text = _id.ToString();

        _surface = surface.Type;

        if (IsWater)
            transform.localPosition -= new Vector3(0f, 3.25f, 0f);


#if UNITY_EDITOR
        gameObject.name = NAME + _id + "__" + key.ToString();
#endif
    }

    public void Select()
    {

        Debug.Log($"{gameObject.name}, water: {IsWater}, gate {IsGate}\n");
    }

    public static KeyDouble operator &(Hexagon a, Hexagon b) => a._key & b._key;

    //public void SetNewPosition(Vector2Int index)
    //{
    //    _key = index;
    //    _thisTransform.localPosition = new(_offset.x * 0.5f * index.x, 0, _offset.y * index.y);
    //}
}
