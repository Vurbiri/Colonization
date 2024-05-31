using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour
{
    [SerializeField] private float _radius = 1.75f;

    public Vector2Int Key => _key;

    private SphereCollider _collider;
    private Vector2Int _key;
    private List<Hexagon> _hexagons = new(COUNT);
    private List<Crossroad> _neighbors;

    private const int COUNT = 3;
#if UNITY_EDITOR
    private const string NAME = "Crossroad_";
#endif

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _radius;
    }

    public void Initialize(Vector2Int key)
    {
        _key = key;

#if UNITY_EDITOR
        gameObject.name = NAME + _key.ToString();
#endif
    }

    public bool Setup()
    {
        if (_hexagons.Count == 1)
        {
            _hexagons[0].RemoveCrossroad(this);
            return false;
        }
        
        if (_hexagons.Count == 2) 
        {
            _collider.enabled = false;
            _hexagons = null;
            return true;
        }

        _collider.enabled = true;
        _neighbors = new(COUNT);

        HashSet<Crossroad> set;
        for(int i = 0; i < COUNT; i++)
        {
            set = new(_hexagons[i].Crossroad);
            set.Remove(this);
            set.IntersectWith(_hexagons.Next(i).Crossroad);
            _neighbors.AddRange(set);
            //_neighbors.Add(set.GetValue(0));
        }

        return true;
    }

    public void AddHexagon(Hexagon hexagon)
    {
        _hexagons.Add(hexagon);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}
