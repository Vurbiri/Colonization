using NaughtyAttributes;
using UnityEngine;
using static CONST;

[RequireComponent(typeof(MeshFilter))]
public class ForestGenerator : MonoBehaviour
{
    [SerializeField, Range(0.01f, 0.3f)] private float _offsetY = 0.1f;
    [SerializeField, Range(1f, 2.5f)] private float _density = 1.9f;
    [SerializeField, Range(0.1f, 1f)] private float _ratioSize = 0.8f;
    [SerializeField, MinMaxSlider(-0.7f, 0.7f)] private Vector2 _offsetRange = new(-0.3f, 0.3f);
    [Space]
    [SerializeField] private Spruce _spruce;
    [Space]
    [SerializeField] private string _nameSuffixFile = "001";
    [SerializeField] private string _path = "Assets/Import/";

    private const float PI = Mathf.PI;
    private const string NAME_MESH = "ForestMesh";

    [Button]
    public void Generate()
    {
        MeshFilter mesh = GetComponent<MeshFilter>();
        CustomMesh customMesh = new(NAME_MESH, HEX_SIZE * Vector2.one);

        float step = _spruce.RadiusAvg * _density, radius = step;
        float angle, angleStep, offsetAngle;
        float x, z;

        customMesh.AddPrimitives(_spruce.Create(new(step * URandom.Range(_offsetRange), _offsetY, step * URandom.Range(_offsetRange))));
        while (radius < HEX_HEIGHT * _ratioSize)
        {
            angle = 0f;
            angleStep = TAU * step / (2f * PI * radius);
            offsetAngle = Random.Range(0f, angleStep);
            while (angle < TAU)
            {
                x = Mathf.Cos(angle + offsetAngle) * radius + step * URandom.Range(_offsetRange);
                z = Mathf.Sin(angle + offsetAngle) * radius + step * URandom.Range(_offsetRange);
                customMesh.AddPrimitives(_spruce.Create(new(x, _offsetY, z)));
                angle += angleStep;
            }

            radius += step;
        }

        mesh.sharedMesh = customMesh.ToMesh();
        mesh.sharedMesh.Optimize();

        #region Local: CheckCoordinates(...)
        //=================================
        //bool CheckCoordinates(float x, float y)
        //{
        //    float max = HEX_HEIGHT * _ratioSize, b = HEX_RADIUS * _ratioSize, k = -0.5f * b / max;
        //    x = Mathf.Abs(x);
        //    y = Mathf.Abs(y);
        //    if (x > max || y > k * x + b || y > max)
        //        return false;

        //    return true;
        //}
        #endregion
    }

    [Button]
    public void SaveMesh() => UnityEditor.AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, _path + NAME_MESH + "_" + _nameSuffixFile + ".mesh");

    #region Nested: Profile
    //*******************************************************
    [System.Serializable]
    private class Spruce
    {
        [SerializeField, Range(1f, 3f)] private float _heightBase = 1.25f;
        [SerializeField, Range(0.5f, 2f)] private float _radiusBase = 1.1f;
        [Space]
        [SerializeField, MinMaxSlider(0.5f, 1.5f)] private Vector2 _sizeRatioRange = new(0.65f, 1.15f);
        [SerializeField, Range(0.1f, 1f)] private float _sizeRatioBorder = 0.88f;
        [Space]
        [SerializeField, MinMaxSlider(3, 7)] private Vector2Int _countVertexRange = new(5, 6);
        [SerializeField] private bool _allBranches = true;
        [Space]
        [SerializeField, Range(0.5f, 1f)] private float _ratioNextPos = 0.75f;
        [SerializeField, Range(0.25f, 0.8f)] private float _ratioNextSize = 0.7f;
        [Space]
        [SerializeField, MinMaxSlider(0.1f, 1f)] private Vector2 _colorRange = new(0.5f, 1f);

        public float RadiusAvg => (_sizeRatioRange.x + _sizeRatioRange.y) * 0.5f * _radiusBase;

        private Color _color = Color.white;
        private const int MIN_COUNT = 3, MAX_COUNT = 4;

        public Pyramid[] Create(Vector3 position)
        {
            _color.SetRandMono(_colorRange);
            float sizeRatio = URandom.Range(_sizeRatioRange);
            int countVertex = URandom.RangeIn(_countVertexRange), count = sizeRatio < _sizeRatioBorder && URandom.IsTrue() ? MIN_COUNT : MAX_COUNT;
            float height = _heightBase * sizeRatio, radius = _radiusBase * sizeRatio;

            Pyramid[] pyramids = new Pyramid[count];

            for (int i = 0; i < count; i++)
            {
                pyramids[i] = new(_color, position, radius, height, Random.Range(0f, PI / countVertex), _allBranches ? countVertex : URandom.RangeIn(_countVertexRange));

                position.y += height * _ratioNextPos;
                height *= _ratioNextSize;
                radius *= _ratioNextSize;
            }

            return pyramids;
        }
    }
    #endregion
}
