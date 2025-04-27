//Assets\Vurbiri.UI\Runtime\UIElements\VSelectable.cs
using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.SELECTABLE, VUI_CONST_ED.SELECTABLE_ORDER)]
    [ExecuteAlways, SelectionBase, DisallowMultipleComponent]
#endif
    public class VSelectable : AVSelectable
    {
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (transition != Transition.ColorTint)
            {
                base.DoStateTransition(state, instant);
                return;
            }

            int intState = (int)state;
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

#if UNITY_EDITOR
            if (!Application.isPlaying) { DoStateTransition_Editor(intState, targetColor); return; }
#endif

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].CrossFadeColor(intState, targetColor, duration);
        }

#if UNITY_EDITOR
        private void DoStateTransition_Editor(int intState, Color targetColor)
        {
            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (_targetGraphics[i].IsValid)
                    _targetGraphics[i].SetColor(intState, targetColor);
        }
#endif
    }
}

