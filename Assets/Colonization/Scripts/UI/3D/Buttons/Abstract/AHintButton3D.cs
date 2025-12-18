using System;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class AHintButton3D : AHintButton
    {
        protected GameObject _thisGameObject;

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            base.InternalInit(HintId.World, 0.505f);
        }

        protected virtual void InternalInit(Action action, bool active)
        {
            base.InternalInit(HintId.World, action, 0.505f);

            _thisGameObject = gameObject;
            _thisGameObject.SetActive(active);
        }
    }
}
