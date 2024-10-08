using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ASurfaceCreated : ASurface
    {
        [SerializeField] protected ASurfaceGenerator _generator;
        [Space]
        [SerializeField, Range(0.1f, 1f)] protected float _ratioSize = 0.8f;

        public override void Init()
        {
            StartCoroutine(Init_Coroutine());

            #region Local: Init_Coroutine()
            //=================================
            IEnumerator Init_Coroutine()
            {
                yield return StartCoroutine(_generator.Generate_Coroutine(CONST.HEX_RADIUS_IN * _ratioSize));

                Destroy(_generator);
                Destroy(this);

                yield return null;

                GC.Collect();
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
