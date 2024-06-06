using System.Collections.Generic;
using UnityEngine;

public class HexagonRoad : MonoBehaviour
{
    [SerializeField] private MeshRenderer _roadRenderer;
    [SerializeField] private Transform _roadTransform;
    [Space]
    [SerializeField] private float _offsetY = 0.05f;

    private static readonly Dictionary<Key, Quaternion> turns = new(CONST.HEX_SIDE);

    static HexagonRoad()
    {
        for (int i = 0; i < CONST.HEX_SIDE; i++)
            turns.Add(CONST.HEX_NEAR[i], Quaternion.Euler(-60f * i * Vector3.up));
    }

    private void Awake()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        _roadTransform.localPosition = new(CONST.HEX_SIZE * 0.5f, _offsetY, 0f);
    }

    public void Initialize(Key key)
    {
        transform.localRotation = turns[key];
    }    
}
