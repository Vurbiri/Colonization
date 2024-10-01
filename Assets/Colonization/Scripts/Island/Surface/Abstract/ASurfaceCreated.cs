using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ASurfaceCreated : ASurface
    {
        [SerializeField] protected ASurfaceGenerator _generator;
        [Space]
        [SerializeField, Range(0.1f, 1f)] protected float _ratioSize = 0.8f;

        public override void Initialize()
        {
            StartCoroutine(Initialize_Coroutine());

            #region Local: Initialize_Coroutine()
            //=================================
            IEnumerator Initialize_Coroutine()
            {
                yield return StartCoroutine(_generator.Generate_Coroutine(CONST.HEX_RADIUS_IN * _ratioSize));

                Destroy(_generator);
                Destroy(this);
            }
            #endregion
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_generator == null)
                _generator = GetComponentInChildren<ASurfaceGenerator>();
        }
#endif
    }
}
