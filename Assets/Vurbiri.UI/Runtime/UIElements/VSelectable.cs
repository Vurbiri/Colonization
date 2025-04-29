//Assets\Vurbiri.UI\Runtime\UIElements\VSelectable.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.SELECTABLE, VUI_CONST_ED.SELECTABLE_ORDER)]
    [ExecuteAlways, SelectionBase, DisallowMultipleComponent]
#endif
    public partial class VSelectable : Selectable
    {
        [SerializeField] protected Graphic _interactableIcon;
        [SerializeField] private bool _alphaCollider = false;
        [SerializeField, Range(0.01f, 1f)] private float _threshold = 0.1f;
        [SerializeField] protected List<TargetGraphic> _targetGraphics = new();
        [SerializeField] private bool _isScaling;
        [SerializeField] private RectTransform _targetTransform;
        [SerializeField] private ScaleBlock _scaleBlock = ScaleBlock.defaultScaleBlock;

        private ScaleTween _scaleTween = new();

        public new bool interactable
        {
            get => base.interactable;
            set
            {
                if (base.interactable == value) return;

                base.interactable = value;
                if (_interactableIcon != null)
                    _interactableIcon.CrossFadeAlpha(value ? 0f : 1f, colors.fadeDuration, true);
            }
        }

        public Graphic InteractableIcon => _interactableIcon;
        public int TargetGraphicCount => _targetGraphics.Count;

        public Graphic GetTargetGraphic(int index) => _targetGraphics[index].Graphic;

        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (!_targetGraphics[i].Validate()) 
                    _targetGraphics.RemoveAt(i);

            _targetGraphics.TrimExcess();

            if (_targetGraphics.Count > 0)
                targetGraphic = _targetGraphics[0];

            _scaleTween = _scaleTween.ReCreate(this, _targetTransform);
        }

        protected override void Start()
        {
            base.Start();

            if (_interactableIcon != null)
                _interactableIcon.canvasRenderer.SetAlpha(base.interactable ? 0f : 1f);

            Image image = targetGraphic as Image;
            if (_alphaCollider && image != null && image.sprite != null && image.sprite.texture.isReadable)
                image.alphaHitTestMinimumThreshold = _threshold;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy) return;

            int intState;
            Vector3 targetScale;
            Color targetColor;
            Sprite targetSprite;

            switch (state)
            {
                case SelectionState.Normal:
                    intState = 0;
                    targetScale = _scaleBlock.normal;
                    targetColor = colors.normalColor;
                    targetSprite = null;
                    break;
                case SelectionState.Highlighted:
                    intState = 1;
                    targetScale = _scaleBlock.highlighted;
                    targetColor = colors.highlightedColor;
                    targetSprite = spriteState.highlightedSprite;
                    break;
                case SelectionState.Pressed:
                    intState = 2;
                    targetScale = _scaleBlock.pressed;
                    targetColor = colors.pressedColor;
                    targetSprite = spriteState.pressedSprite;
                    break;
                case SelectionState.Selected:
                    intState = 3;
                    targetScale = _scaleBlock.selected;
                    targetColor = colors.selectedColor;
                    targetSprite = spriteState.selectedSprite;
                    break;
                case SelectionState.Disabled:
                    intState = 4;
                    targetScale = _scaleBlock.disabled;
                    targetColor = colors.disabledColor;
                    targetSprite = spriteState.disabledSprite;
                    break;
                default:
                    intState = -1;
                    targetScale = Vector3.zero;
                    targetColor = Color.black;
                    targetSprite = null;
                    break;
            }

            switch (transition)
            {
                case Transition.None:
                    StartScaleTween(targetScale, instant ? 0f : _scaleBlock.fadeDuration);
                    break;
                case Transition.ColorTint:
                    StartColorTween(intState, targetScale, targetColor * colors.colorMultiplier, instant ? 0f : colors.fadeDuration);
                    break;
                case Transition.SpriteSwap:
                    DoSpriteSwap(targetScale, targetSprite, instant ? 0f : _scaleBlock.fadeDuration);
                    break;
                case Transition.Animation:
                    Debug.LogWarning("Animation is not supported");
                    break;
            }
        }

        protected virtual void StartScaleTween(Vector3 targetScale, float duration)
        {
            _scaleTween.Set(targetScale, duration);
        }

        protected virtual void StartColorTween(int intState, Vector3 targetScale, Color targetColor, float duration)
        {
            _scaleTween.Set(targetScale, duration);

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].CrossFadeColor(intState, targetColor, duration);

        }

        protected virtual void DoSpriteSwap(Vector3 targetScale, Sprite targetSprite, float duration)
        {
            _scaleTween.Set(targetScale, duration);

            if (image != null)
                image.overrideSprite = targetSprite;
        }

        protected override void OnEnable()
        {
            if(_isScaling) _scaleTween.Enable();
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _scaleTween.Disable();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!Application.isPlaying)
            {
                _scaleTween = _scaleTween.ReCreate(this, _targetTransform, _isScaling);
                if (!_isScaling && _targetTransform != null)
                    _targetTransform.localScale = Vector3.one;

                for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                    _targetGraphics[i].Validate();
            }

            base.OnValidate();
        }

        protected override void Reset()
        {
            _targetGraphics = new()
            {
                GetComponent<Graphic>()
            };
        }
#endif
    }
}

