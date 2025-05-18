//Assets\Vurbiri.UI\Runtime\UIElements\Hint\CanvasHint.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class CanvasHint : AHint
    {
        [SerializeField] private float _edgeX = 5f;

        private RectTransform _parentRectTransform;

        public override void Init(Color backColor, Color textColor)
        {
            base.Init(backColor, textColor);

            _parentRectTransform = (RectTransform)_backTransform.parent;
        }

        protected override void SetPosition(Vector3 position, Vector3 offset)
        {
            position = _parentRectTransform.InverseTransformPoint(position);

            Vector2 thisSize = _backTransform.sizeDelta * 0.5f;
            Vector2 parentSize = _parentRectTransform.sizeDelta * 0.5f;

            float delta = position.x - thisSize.x - _edgeX + parentSize.x;
            if (delta > 0f)
            {
                delta = position.x + thisSize.x + _edgeX - parentSize.x;
                if (delta < 0f)
                    delta = 0f;
            }

            offset.x -= delta;
            offset.y += thisSize.y;

            if (position.y > 0f)
                offset.y *= -1f;

            _backTransform.localPosition = position + offset;
        }
    }
}
