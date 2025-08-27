using System;
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
            _generator.Generate(CONST.HEX_RADIUS_IN * _ratioSize);
            _generator.Dispose();
            Destroy(this);
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
