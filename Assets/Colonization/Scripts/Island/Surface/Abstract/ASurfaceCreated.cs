//Assets\Colonization\Scripts\Island\Surface\Abstract\ASurfaceCreated.cs
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

        public override void Init(bool oneFrame)
        {
            if (!oneFrame)
            {
                StartCoroutine(Init_Cn());
                return;
            }

            _generator.Generate(CONST.HEX_RADIUS_IN * _ratioSize);
            _generator.Dispose();
            Destroy(this);
        }

        protected IEnumerator Init_Cn()
        {
            yield return StartCoroutine(_generator.Generate_Cn(CONST.HEX_RADIUS_IN * _ratioSize));

            _generator.Dispose();
            Destroy(this);

            yield return null;

            GC.Collect();
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
