using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class CanvasHint : Hint
    {
        [Space]
        [SerializeField] private Vector2 _edge;
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

        protected override void SetPosition(RectTransform rectTransform, HintOffset offset)
        {
            var position = offset.GetCenterPosition(_canvasRectTransform.InverseTransformPoint(rectTransform.position));
            var hintSize = _backTransform.sizeDelta * 0.5f;
            var viewSize = _canvasRectTransform.sizeDelta * 0.5f - (hintSize + _edge);
            var deltaY = offset.GetDeltaY(hintSize.y);
            var deltaX = viewSize.x - position.x;

            if (deltaX > 0f)
            {
                deltaX = -viewSize.x - position.x;
                if (deltaX < 0f)
                    deltaX = 0f;
            }
            position.x += deltaX;

            if (position.y < viewSize.y - deltaY)
                position.y += deltaY;
            else
                position.y -= deltaY;

            _backTransform.localPosition = position;
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
