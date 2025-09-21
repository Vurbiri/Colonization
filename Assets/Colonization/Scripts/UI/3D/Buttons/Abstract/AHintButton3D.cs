using System;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class AHintButton3D : AHintButton<WorldHint>
    {
        protected GameObject _thisGameObject;

        protected virtual void InternalInit(WorldHint hint, Action action, bool active)
        {
            base.InternalInit(hint, action, 0.505f);

            _thisGameObject = gameObject;
            _thisGameObject.SetActive(active);
        }
    }
}
