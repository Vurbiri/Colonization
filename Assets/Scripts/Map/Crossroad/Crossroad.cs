using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable
{
    [SerializeField] private float _radius = 1.75f;

    public Key Key => _key;
    public Vector3 Position { get; private set; }
    public bool IsGate => _isGate;
    public bool IsWater => _isWater;
    public HashSet<Road> Roads => _roads;

    private Key _key;
    private bool _isGate = false, _isWater = true;
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly HashSet<Road> _roads = new(COUNT);

    private Action<Crossroad> _actionSelect;

    private const int COUNT = 3;
#if UNITY_EDITOR
    private const string NAME = "Crossroad_";
#endif

    public void Initialize(Key key, Action<Crossroad> action)
    {
        _key = key;
        Position = transform.position;
        _actionSelect = action;
        GetComponent<SphereCollider>().radius = _radius;

#if UNITY_EDITOR
        gameObject.name = NAME + Key.ToString();
#endif
    }

    public void AddHexagon(Hexagon hex)
    { 
        _hexagons.Add(hex);

        _isGate = _isGate || hex.IsGate;
        _isWater = _isWater && hex.IsWater;
    }

    public void Select()
    {
        if(!_isWater)
            _actionSelect(this);
    }

#if UNITY_EDITOR
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, _radius);
    //}
#endif
}

