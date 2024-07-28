using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    public class CmSelectable : Selectable
    {
        [SerializeField] private GameObject _interactableIcon;
        [SerializeField] private bool _alfaCollider = false;
        [SerializeField, Range(0.01f, 1f)] private float _threshold = 0.1f;
        [SerializeField] private Graphic[] _targetGraphics;

        public Graphic[] TargetGraphics => _targetGraphics;

        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
                if (_interactableIcon != null)
                    _interactableIcon.SetActive(!value);
            }
        }

        public void SetActive(bool active) => gameObject.SetActive(active);

        protected override void Start()
        {
            base.Start();

            if (_targetGraphics.Length > 0)
            {
                targetGraphic = _targetGraphics[0];
            }
            else if (targetGraphic != null)
            {
                _targetGraphics = new Graphic[1];
                _targetGraphics[0] = targetGraphic;
            }

            if (image != null && image.sprite != null && image.sprite.texture.isReadable)
                image.alphaHitTestMinimumThreshold = !_alfaCollider ? _threshold : 0f;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {

            if (transition != Transition.ColorTint)
            {
                base.DoStateTransition(state, instant);
                return;
            }

            if (!gameObject.activeInHierarchy || _targetGraphics == null)
                return;

            Color targetColor = state switch
            {
                SelectionState.Normal => colors.normalColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Selected => colors.selectedColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Disabled => colors.disabledColor,
                _ => Color.black
            };

            foreach (Graphic graphic in _targetGraphics)
                StartColorTween(graphic);

            #region Local: StartColorTween(...)
            //=================================
            void StartColorTween(Graphic targetGraphic)
            {
                if (targetGraphic == null)
                    return;

                targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
            #endregion
        }
    }

    
}
