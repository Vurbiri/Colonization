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
        [SerializeField, GetComponentInChildren] private Animator _windmillAnimator;
        [Space]
        [SerializeField] private Mesh _altWindmillMesh;
        [SerializeField] private float _windmillOffsetDistance = 0.6f;
        [SerializeField] private float _windmillSpeedAnimRange = 0.2f;
        [SerializeField] private RFloat _windmillPlayRange = new(0.2f, 1.22f);

        private const string NAME_ANIM_PARAMETER = "Play";

        public override void Initialize()
        {
            float size = CONST.HEX_RADIUS_IN * _ratioSize;

            transform.localRotation = Quaternion.Euler(0f, _offsetAngle + 60f * Random.Range(0, 6) + 30f, 0f);
            StartCoroutine(_generator.Generate_Coroutine(size));

            if(Chance.Rolling())
                _windmillMeshFilter.sharedMesh = _altWindmillMesh;
            _windmillMeshFilter.transform.localPosition = new(0f, 0f, size - _windmillOffsetDistance);

            StartCoroutine(WindmillPlay_Coroutine());

            #region Local: WindmillPlay_Coroutine()
            //=================================
            IEnumerator WindmillPlay_Coroutine()
            {
                yield return new WaitForSeconds(_windmillPlayRange);
                _windmillAnimator.SetTrigger(NAME_ANIM_PARAMETER);
                _windmillAnimator.speed = 1f + Random.Range(-_windmillSpeedAnimRange, _windmillSpeedAnimRange);
            }
            #endregion
        }
    }
}
