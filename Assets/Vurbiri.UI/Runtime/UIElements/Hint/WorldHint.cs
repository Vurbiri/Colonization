using UnityEngine;

namespace Vurbiri.UI
{
    sealed public class WorldHint : AHint
    {
        protected override void SetPosition(Transform transform, Vector3 offset)
        {
            offset.y += _backTransform.sizeDelta.y * 0.5f;
            _backTransform.localPosition = transform.localPosition + offset;
        }
    }
}
