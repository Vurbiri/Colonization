using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Village : ASurfaceCreated
    {
        [Space]
        [SerializeField] private RMFloat _offsetAngle = 15f;
        [Space]
        [SerializeField] private MeshFilter _windmillMeshFilter;
        [Space]
        [SerializeField] private Mesh _meshWindmill01;
        [SerializeField] private Mesh _meshWindmill02;
        [SerializeField] private float _windmillOffsetDistance = 0.6f;

        private static Chance chanceAltMesh = new(50);

        public override void Init()
        {
            float size = CONST.HEX_RADIUS_IN * _ratioSize;
            transform.localRotation = Quaternion.Euler(0f, _offsetAngle + 60f * Random.Range(0, 6) + 30f, 0f);
                        
            _windmillMeshFilter.sharedMesh = chanceAltMesh.Select(_meshWindmill01, _meshWindmill02);
            _windmillMeshFilter.transform.localPosition = new(0f, 0f, size - _windmillOffsetDistance);

            StartCoroutine(Init_Coroutine(size));

            #region Local: Init_Coroutine(), WindmillPlay_Coroutine()
            //=================================
            IEnumerator Init_Coroutine(float size)
            {
                yield return StartCoroutine(_generator.Generate_Coroutine(size));

                Destroy(_generator);
                Destroy(this);

                yield return null;

                System.GC.Collect();
            }
            #endregion
        }
    }
}
