using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(MeshRenderer))] 
public class Hexagon : MonoBehaviour
{

    [SerializeField] private Vector2Int _index = Vector2Int.zero;
    [SerializeField] private Vector2 _offset;
    [Space]
    [SerializeField] private Material _ground;
    [SerializeField] private Material _water;

    public Vector2Int Index => _index;
    
    private Transform _thisTransform;
    private MeshRenderer _thisRenderer;

    private void Awake()
    {
        _thisTransform = transform;
        _thisRenderer = GetComponent<MeshRenderer>();  
    }

    public void Setup(Vector2 offset, bool water)
    {
        _offset = offset;
        _thisRenderer.sharedMaterial = water ? _water : _ground;

        Vector3 position = _thisTransform.localPosition;

        _index.y = Mathf.RoundToInt(position.z / _offset.y);
        _index.x = Mathf.RoundToInt(2f * position.x / _offset.x);
    }

    public void SetNewPosition(Vector2Int indexNew)
    {
        _index = indexNew;
        _thisTransform.localPosition = new(_offset.x * 0.5f * indexNew.x, 0, _offset.y * indexNew.y);
    }

}
