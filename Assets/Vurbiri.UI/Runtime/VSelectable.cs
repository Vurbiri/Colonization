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

        sealed protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            int intState = (int)state;

            if (transition != Transition.ColorTint)
            {
                OnStateTransition(intState, Color.white, 0f);
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
            


#if UNITY_EDITOR
            if (!Application.isPlaying) { DoStateTransition_Editor(intState, targetColor); return; }
#endif

            OnStateTransition(intState, targetColor, duration);

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].CrossFadeColor(intState, targetColor, duration);
        }

        protected virtual void OnStateTransition(int intState, Color targetColor, float duration)
        {

        }

#if UNITY_EDITOR
        private void DoStateTransition_Editor(int intState, Color targetColor)
        {
            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (_targetGraphics[i].IsValid)
                    _targetGraphics[i].SetColor(intState, targetColor);

            OnStateTransition(intState, targetColor, 0f);
        }
#endif
    }
}

