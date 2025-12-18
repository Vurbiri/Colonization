using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class CanvasHint : Hint
    {
        [SerializeField] private float _edgeX;
        [Space]
        [SerializeField] private RectTransform _canvasRectTransform;

        public override bool Init()
        {
            if (base.Init())
            {
                if (_canvasRectTransform != transform.parent)
                    transform.SetParent(_canvasRectTransform);

                transform.SetAsLastSibling();
                return true;
            }
            return false;
        }

        protected override void SetPosition(Transform transform, Vector3 offset)
        {
            var position = _canvasRectTransform.InverseTransformPoint(transform.position);
            var thisSize = _backTransform.sizeDelta * 0.5f;
            var parentSize = _canvasRectTransform.sizeDelta * 0.5f;

            position.x += offset.x;
            float delta = (position.x - thisSize.x - _edgeX) + parentSize.x;
            if (delta > 0f)
            {
                delta = (position.x + thisSize.x + _edgeX) - parentSize.x;
                if (delta < 0f)
                    delta = 0f;
            }

            offset.x = -delta;
            offset.y += thisSize.y;

            if (position.y > 0f)
                offset.y = -offset.y;

            _backTransform.localPosition = position + offset;
        }

#if UNITY_EDITOR
        public override void UpdateVisuals_Ed(Color backColor, Color textColor)
        {
            base.UpdateVisuals_Ed(backColor, textColor);

            if (_canvasRectTransform != null && _canvasRectTransform != transform.parent)
                transform.SetParent(_canvasRectTransform);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
                return;

            if (_canvasRectTransform == null)
            {
                var canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                _canvasRectTransform = canvas.GetComponent<RectTransform>();
            }

            if (_canvasRectTransform != null && _canvasRectTransform != transform.parent)
                transform.SetParent(_canvasRectTransform);
        }
#endif
    }
}
