//Assets\Vurbiri.UI\Runtime\VSelectable.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Selectable", 35)]
    [ExecuteAlways, SelectionBase, DisallowMultipleComponent]
    public partial class VSelectable : Selectable
    {
        [SerializeField] protected Graphic _interactableIcon;
        [SerializeField] private bool _alphaCollider = false;
        [SerializeField, Range(0.01f, 1f)] private float _threshold = 0.1f;
        [SerializeField] protected List<TargetGraphic> _targetGraphics = new();

        public Graphic InteractableIcon => _interactableIcon;
        public IReadOnlyList<TargetGraphic> TargetGraphics => _targetGraphics;

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
        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (!_targetGraphics[i].IsValid) _targetGraphics.RemoveAt(i);

            _targetGraphics.TrimExcess();

            if (_targetGraphics.Count > 0)
                targetGraphic = _targetGraphics[0];
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

        protected Color CurrentColor
        {
            get
            {
                if (transition != Transition.ColorTint) return Color.white;
                return currentSelectionState switch
                {
                    SelectionState.Normal => colors.normalColor,
                    SelectionState.Highlighted => colors.highlightedColor,
                    SelectionState.Selected => colors.selectedColor,
                    SelectionState.Pressed => colors.pressedColor,
                    SelectionState.Disabled => colors.disabledColor,
                    _ => Color.white
                };
            }
        }

        sealed protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (transition != Transition.ColorTint)
            {
                base.DoStateTransition(state, instant);
                return;
            }

            
            Color targetColor = state switch
            {
                SelectionState.Normal => colors.normalColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Selected => colors.selectedColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Disabled => colors.disabledColor,
                _ => Color.black
            };
            float duration = instant ? 0f : colors.fadeDuration;
            int intState = (int)state;


#if UNITY_EDITOR
            if (!Application.isPlaying) { DoStateTransition_Editor(intState, targetColor, duration, instant); return; }
#endif

            OnStateTransition(intState, targetColor, duration, instant);

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].CrossFadeColor(intState, targetColor, duration);
        }

        protected virtual void OnStateTransition(int intState, Color targetColor, float duration, bool instant)
        {

        }

#if UNITY_EDITOR
        private void DoStateTransition_Editor(int intState, Color targetColor, float duration, bool instant)
        {
            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (_targetGraphics[i].IsValid)
                    _targetGraphics[i].SetColor(intState, targetColor);

            OnStateTransition(intState, targetColor, duration, instant);
            return;
        }
#endif
    }
}

