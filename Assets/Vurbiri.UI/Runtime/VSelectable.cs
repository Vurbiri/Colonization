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
                base.interactable = value;
                if (_interactableIcon != null)
                    _interactableIcon.CrossFadeAlpha(!value ? 1f : 0f, colors.fadeDuration, true);
            }
        }

        protected override void Start()
        {
            base.Start();

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
            {
                if (_targetGraphics[i] == null)
                    _targetGraphics.RemoveAt(i);
            }
            _targetGraphics.TrimExcess();

            if (_targetGraphics.Count > 0)
                targetGraphic = _targetGraphics[0];

            Image image = targetGraphic as Image;
            if (_alfaCollider && image != null && image.sprite != null && image.sprite.texture.isReadable)
                image.alphaHitTestMinimumThreshold = _threshold;
        }

        sealed protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (transition != Transition.ColorTint | _targetGraphics.Count < 1)
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

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                CrossFadeColorNotPlaying_Editor(targetColor, instant ? 0f : colors.fadeDuration);
                return;
            }
#endif
            CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration);
        }

        protected virtual void CrossFadeColor(Color targetColor, float duration)
        {
            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].CrossFadeColor(targetColor, duration, true, true);
        }

#if UNITY_EDITOR

        protected virtual void CrossFadeColorNotPlaying_Editor(Color targetColor, float duration)
        {
            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (_targetGraphics[i] != null)
                    _targetGraphics[i].CrossFadeColor(targetColor, duration, true, true);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            _targetGraphics ??= new();

            if (_targetGraphics.Count > 0 && _targetGraphics[0] != null)
                targetGraphic = _targetGraphics[0];
            else if (targetGraphic != null)
                _targetGraphics.Add(targetGraphic);
        }
#endif
    }
}

