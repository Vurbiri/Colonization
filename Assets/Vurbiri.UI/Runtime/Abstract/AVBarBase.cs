//Assets\Vurbiri.UI\Runtime\Abstract\AVBarBase.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [ExecuteAlways]
    public abstract class AVBarBase : MonoBehaviour
#if UNITY_EDITOR
        , ICanvasElement
#endif
    {
        private const int HORIZONTAL = 0, VERTICAL = 1;

        [SerializeField] private RectTransform _fillRect;
        [SerializeField] private Direction _direction = Direction.LeftToRight;
        [SerializeField] private bool _useGradient = true;
        [SerializeField] private Gradient _gradient = new();

        protected float _normalizedValue;
        private int _axis;
        private bool _reverseValue;

        private RectTransform _thisRectTransform;
        private CanvasRenderer _fillRenderer;
        private Image _fillImage;
        private RectTransform _fillContainerRect;

        private DrivenRectTransformTracker _tracker;

        #region Properties
        public RectTransform FillRect
        {
            get => _fillRect;
            set
            {
                if (_fillRect != value)
                {
                    _fillRect = value;
                    UpdateFillRectReferences();
                    UpdateVisuals();
                }
            }
        }
        public Direction Direction
        {
            get => _direction;
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    UpdateDirection(value, true);
                    UpdateVisuals();
                }
            }
        }
        
        public bool UseGradient
        {
            get => _useGradient;
            set
            {
                if (_useGradient != value)
                {
                    _useGradient = value;
                    UpdateColor();
                }
            }
        }
        public Gradient Gradient
        {
            get => _gradient;
            set
            {
                if (_gradient != value)
                {
                    _gradient = value;
                    UpdateColor();
                }
            }
        }
        #endregion

        public void UpdateColor()
        {
            if (_fillRenderer != null)
                _fillRenderer.SetColor(_useGradient & _gradient != null ? _gradient.Evaluate(_normalizedValue) : Color.white);
        }

        #region Update...
        private void UpdateFillRectReferences()
        {
            _fillContainerRect = null;
            _fillRenderer = null;
            _fillImage = null;

            if (_fillRect != null & _fillRect != _thisRectTransform && _fillRect.parent != null)
            {
                _fillContainerRect = (RectTransform)_fillRect.parent;

                if (_fillRect.TryGetComponent<Graphic>(out var graphic))
                {
                    _fillRenderer = graphic.canvasRenderer;
                    _fillImage = graphic as Image;
                }
            }
            else
            {
                _fillRect = null;
            }
        }

        protected void UpdateVisuals()
        {
            _tracker.Clear();

            if (_fillContainerRect != null)
            {
                _tracker.Add(this, _fillRect, DrivenTransformProperties.Anchors);
                Vector2 anchorMin = Vector2.zero;
                Vector2 anchorMax = Vector2.one;

                if (_fillImage != null && _fillImage.type == Image.Type.Filled)
                {
                    _fillImage.fillAmount = _normalizedValue;
                }
                else
                {
                    if (_reverseValue)
                        anchorMin[_axis] = 1f - _normalizedValue;
                    else
                        anchorMax[_axis] = _normalizedValue;
                }

                if (_useGradient && _fillRenderer != null & _gradient != null)
                    _fillRenderer.SetColor(_gradient.Evaluate(_normalizedValue));

                _fillRect.anchorMin = anchorMin;
                _fillRect.anchorMax = anchorMax;
            }
        }

        private void UpdateDirection(Direction direction, bool flipLayout)
        {
            int oldAxis = _axis;
            bool oldReverse = _reverseValue;

            _axis = (direction == Direction.LeftToRight | direction == Direction.RightToLeft) ? HORIZONTAL : VERTICAL;
            _reverseValue = direction == Direction.RightToLeft | direction == Direction.TopToBottom;

            if (flipLayout & _axis != oldAxis) RectTransformUtility.FlipLayoutAxes(_thisRectTransform, true, true);
            if (flipLayout & _reverseValue != oldReverse) RectTransformUtility.FlipLayoutOnAxis(_thisRectTransform, _axis, true, true);
        }
        #endregion

        #region Calls ..
        private void Awake()
        {
            _thisRectTransform = (RectTransform)transform;
            UpdateFillRectReferences();
            UpdateDirection(_direction, false);
        }

        private void OnDisable()
        {
            _tracker.Clear();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (isActiveAndEnabled)
                UpdateVisuals();
        }
        #endregion

#if UNITY_EDITOR
        #region ICanvasElement
        public void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout)
                UpdateVisuals();
        }
        public bool IsDestroyed() => this == null;
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion

        protected virtual void OnValidate()
        {
            if (!Application.isPlaying)
            {
                _thisRectTransform = (RectTransform)transform;
                UpdateFillRectReferences();
                UpdateDirection(_direction, false);
                UpdateColor();

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
#endif
    }
}
