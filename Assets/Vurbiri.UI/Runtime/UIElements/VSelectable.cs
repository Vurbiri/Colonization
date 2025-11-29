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
        [SerializeField] protected Graphic _lockIcon;
        [SerializeField] protected List<TargetGraphic> _targetGraphics = new();
        [SerializeField] private bool _scaling;
        [SerializeField] private RectTransform _scalingTarget;
        [SerializeField] private ScaleBlock _scales = ScaleBlock.defaultScaleBlock;

        private ScaleTween _scaleTween = new();

        #region Properties
        public bool Unlock
        {
            get => base.interactable;
            set
            {
                if (base.interactable != value)
                {
                    base.interactable = value;
                    if (_lockIcon != null)
                        _lockIcon.CrossFadeAlpha(value ? 0f : 1f, colors.fadeDuration, true);
                }
            }
        }
        public bool Lock
        {
            get => !base.interactable;
            set
            {
                if (base.interactable == value)
                {
                    base.interactable = !value;
                    if (_lockIcon != null)
                        _lockIcon.CrossFadeAlpha(value ? 1f : 0f, colors.fadeDuration, true);
                }
            }
        }
        public Graphic LockIcon
        {
            get => _lockIcon;
            set
            {
                if (_lockIcon != value)
                {
                    _lockIcon = value;
                    if (_lockIcon != null)
                        _lockIcon.CrossFadeAlpha(base.interactable ? 0f : 1f, colors.fadeDuration, true);
                }
            }
        }
        public bool Scaling
        {
            get => _scaling;
            set
            {
                if (_scaling != value)
                {
                    _scaling = value;
                    if(isActiveAndEnabled)
                        _scaleTween.SetActive(value);
                }
            }
        }
        public RectTransform ScalingTarget
        {
            get => _scalingTarget;
            set
            {
                if (_scalingTarget != value)
                {
                    _scalingTarget = value;
                    _scaleTween.ReCreate(this, _scalingTarget, _scaling && isActiveAndEnabled);
                }
            }
        }
        public ScaleBlock Scales
        {
            get => _scales;
            set 
            { 
                if (value != _scales)
                {
                    _scales = value;
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        DoStateTransition(currentSelectionState, true);
                    else
#endif
                        DoStateTransition(currentSelectionState, false);
                }
            }
        }

        public int TargetGraphicCount => _targetGraphics.Count;
        #endregion

        public Graphic GetTargetGraphic(int index) => _targetGraphics[index].Graphic;

        public void InteractableAndUnlock(bool interactable, bool unlocked)
        {
            base.interactable = interactable & unlocked;
            if (_lockIcon != null)
                _lockIcon.CrossFadeAlpha(unlocked ? 0f : 1f, colors.fadeDuration, true);
        }

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

            _scaleTween = _scaleTween.ReCreate(this, _scalingTarget);
        }

        protected override void Start()
        {
            base.Start();

            if (_lockIcon != null)
                _lockIcon.canvasRenderer.SetAlpha(base.interactable ? 0f : 1f);
        }

        sealed protected override void DoStateTransition(SelectionState state, bool instant)
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
                    targetScale = _scales.normal;
                    targetColor = colors.normalColor;
                    targetSprite = null;
                    break;
                case SelectionState.Highlighted:
                    intState = 1;
                    targetScale = _scales.highlighted;
                    targetColor = colors.highlightedColor;
                    targetSprite = spriteState.highlightedSprite;
                    break;
                case SelectionState.Pressed:
                    intState = 2;
                    targetScale = _scales.pressed;
                    targetColor = colors.pressedColor;
                    targetSprite = spriteState.pressedSprite;
                    break;
                case SelectionState.Selected:
                    intState = 3;
                    targetScale = _scales.selected;
                    targetColor = colors.selectedColor;
                    targetSprite = spriteState.selectedSprite;
                    break;
                case SelectionState.Disabled:
                    intState = 4;
                    targetScale = _scales.disabled;
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
                    StartScaleTween(targetScale, instant ? 0f : _scales.fadeDuration);
                    break;
                case Transition.ColorTint:
                    StartColorTween(intState, targetScale, targetColor, instant ? 0f : colors.fadeDuration);
                    break;
                case Transition.SpriteSwap:
                    DoSpriteSwap(targetScale, targetSprite, instant ? 0f : _scales.fadeDuration);
                    break;
                case Transition.Animation:
                    Errors.Message("Animation is not supported");
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
            if(_scaling) _scaleTween.Enable();
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
                if (transition == Transition.ColorTint && _scales.fadeDuration != colors.fadeDuration)
                    _scales.fadeDuration = colors.fadeDuration;

                _scaleTween ??= new();
                _scaleTween = _scaleTween.ReCreate(this, _scalingTarget, _scaling);
                if (!_scaling && _scalingTarget != null)
                    _scalingTarget.localScale = Vector3.one;

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

