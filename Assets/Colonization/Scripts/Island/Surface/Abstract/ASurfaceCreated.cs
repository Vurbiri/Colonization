using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ASurfaceCreated : ASurface
    {
        [SerializeField, GetComponentInChildren] protected ASurfaceGenerator _generator;
        [Space]
        [SerializeField, Range(0.1f, 1f)] protected float _ratioSize = 0.8f;

        public override void Initialize()
        {
            StartCoroutine(_generator.Generate_Coroutine(CONST.HEX_RADIUS_IN * _ratioSize));
        }
    }
}
