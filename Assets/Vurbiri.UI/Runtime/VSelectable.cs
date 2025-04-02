//Assets\Vurbiri.UI\Runtime\VSelectable.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Selectable", 35)]
    [ExecuteAlways, SelectionBase, DisallowMultipleComponent]
    public class VSelectable : Selectable
    {
        [SerializeField] protected Graphic _interactableIcon;
        [SerializeField] private bool _alfaCollider = false;
        [SerializeField, Range(0.01f, 1f)] private float _threshold = 0.1f;
        [SerializeField] protected List<Graphic> _targetGraphics = new();

        public IReadOnlyList<Graphic> TargetGraphics => _targetGraphics;
        public Graphic InteractableIcon => _interactableIcon;

        public new virtual bool interactable
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
                if (_targetGraphics[i] == null) _targetGraphics.RemoveAt(i);

            _targetGraphics.TrimExcess();

            if (_targetGraphics.Count > 0)
                targetGraphic = _targetGraphics[0];

            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (_interactableIcon != null)
                _interactableIcon.canvasRenderer.SetAlpha(base.interactable ? 0f : 1f);

            Image image = targetGraphic as Image;
            if (_alfaCollider && image != null && image.sprite != null && image.sprite.texture.isReadable)
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

#if UNITY_EDITOR
            if (!Application.isPlaying) { DoStateTransition_Editor(state, instant); return; }
#endif

            if (transition != Transition.ColorTint | _targetGraphics.Count < 1)
            {
                base.DoStateTransition(state, instant);
                return;
            }

            float duration = instant ? 0f : colors.fadeDuration;
            Color targetColor = state switch
            {
                SelectionState.Normal => colors.normalColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Selected => colors.selectedColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Disabled => colors.disabledColor,
                _ => Color.black
            };

            OnStateTransition(state, targetColor, duration, instant);

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].CrossFadeColor(targetColor, duration, true, true);
        }

        protected virtual void OnStateTransition(SelectionState state, Color targetColor, float duration, bool instant)
        {

        }

#if UNITY_EDITOR
        private void DoStateTransition_Editor(SelectionState state, bool instant)
        {
            if (transition != Transition.ColorTint | _targetGraphics.Count < 1)
            {
                base.DoStateTransition(state, instant);
                return;
            }

            float duration = instant ? 0f : colors.fadeDuration;
            Color targetColor = state switch
            {
                SelectionState.Normal => colors.normalColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Selected => colors.selectedColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Disabled => colors.disabledColor,
                _ => Color.black
            };

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (_targetGraphics[i] != null)
                    _targetGraphics[i].canvasRenderer.SetColor(targetColor);

            OnStateTransition(state, targetColor, duration, instant);
            return;
        }
#endif
    }
}

