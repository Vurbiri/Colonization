using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour
{
    [SerializeField] private Vector2Int _index = Vector2Int.zero;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private List<Hexagon> _hexagons = new(3);
    [Space]
    [SerializeField] private float _radius = 1.75f;

    public Vector2Int Index { get => _index; set => _index = value; }
    public Vector2 Offset { get => _offset; set => _offset = value; }

    private Transform _thisTransform;
    private SphereCollider _collider;
    
    private void Awake()
    {
        _thisTransform = transform;

        _collider = GetComponent<SphereCollider>();
        _collider.radius = _radius;
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
