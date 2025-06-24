using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class CanvasHint : AHint
    {
        [SerializeField] private float _edgeX = 5f;
        [Space]
        [SerializeField] private RectTransform _canvasRectTransform;

        public override void Init()
        {
            base.Init();

            if(_canvasRectTransform != transform.parent)
                transform.SetParent(_canvasRectTransform);
        }

        protected override void SetPosition(Vector3 position, Vector3 offset)
        {
            position = _canvasRectTransform.InverseTransformPoint(position);

            Vector2 thisSize = _backTransform.sizeDelta * 0.5f;
            Vector2 parentSize = _canvasRectTransform.sizeDelta * 0.5f;

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

#if UNITY_EDITOR
        public override void UpdateVisuals_Editor(Color backColor, Color textColor)
        {
            base.UpdateVisuals_Editor(backColor, textColor);

            if (_canvasRectTransform != null && _canvasRectTransform != transform.parent)
                transform.SetParent(_canvasRectTransform);
        }


        protected override void OnValidate()
        {
            base.OnValidate();

            if (_canvasRectTransform == null)
            {
                _canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            }
        }
#endif
    }
}
