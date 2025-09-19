using UnityEngine;

namespace Vurbiri.UI
{
    sealed public class WorldHint : AHint
    {
        protected override void SetPosition(Vector3 position, Vector3 offset)
        {
            offset.y += _backTransform.sizeDelta.y;
            _backTransform.position = position + offset;
        }
    }
}
