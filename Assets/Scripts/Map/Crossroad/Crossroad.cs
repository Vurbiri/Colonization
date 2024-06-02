using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<Hexagon> Hexagons => _hexagons;

    public Key _key;
    private bool _isGate = false, _isWater = true;
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly Dictionary<Crossroad, Road> _neighbors = new(COUNT);

    private Action<Crossroad> actionSelect;

    private const int COUNT = 3;
#if UNITY_EDITOR
    private const string NAME = "Crossroad_";
#endif

    public void Initialize(Key key, Action<Crossroad> action)
    {
        _key = key;
        Position = transform.position;
        actionSelect = action;
        GetComponent<SphereCollider>().radius = _radius;

#if UNITY_EDITOR
        gameObject.name = NAME + Key.ToString();
#endif
    }

    public void Setup()
    {
        HashSet<Crossroad> set;
        Hexagon current, next = _hexagons[0];

        for (int i = 0; i < COUNT; i++)
        {
            current = next;
            next = _hexagons.Next(i);

            _isGate = _isGate || current.IsGate;
            _isWater = _isWater && current.IsWater;

            set = new(current.Crossroads);
            set.Remove(this);
            set.IntersectWith(next.Crossroads);

            if(set.Count > 0) 
                _neighbors.Add(set.First(), new(current.IsWater && next.IsWater));
        }
    }

    public void RoadAdd(Crossroad cross, Player player)
    {
        Road road = _neighbors[cross];
        road.type = RoadType.Start;
        road.owner = player;

        road = cross._neighbors[this];
        road.type = RoadType.End;
        road.owner = player;
    }

    public void Select()
    {
        actionSelect(this);


        string s = $"{gameObject.name}, water: {_isWater}, gate {_isGate}\n";
        //foreach (var hexagon in _hexagons)
        //    s += hexagon.gameObject.name + " | ";
        foreach (var neighbor in _neighbors)
            s += neighbor.Value.isWater + " | ";
        Debug.Log(s);
    }

    #region Nested: Surfaces
    //***********************************
    [Serializable]
    private class Road
    {
        public RoadType type;
        public Player owner;
        public readonly bool isWater;

        public Road(RoadType type, Player owner, bool isWater)
        {
            this.type = type;
            this.owner = owner;
            this.isWater = isWater;
        }

        public Road(bool isWater)
        {
            type = RoadType.None;
            owner = Player.None;
            this.isWater = isWater;
        }
    }
    #endregion

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}

