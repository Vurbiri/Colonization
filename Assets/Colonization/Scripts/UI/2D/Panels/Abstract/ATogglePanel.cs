using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class ATogglePanel<TWidget, TButton> : ASinglyPanel<TWidget> where TWidget :AHintWidget where TButton : APanelButton
    {
        [SerializeField] protected VToggle _toggle;
        [Space]
        [SerializeField] protected TButton _buttonPrefab;
        [SerializeField] protected Transform _buttonContainer;

        protected readonly List<TButton> _buttons = new();
        protected InputController _inputController;
        private Coroutine _coroutineToggle;

        protected void InitToggle(IReactive<int> current)
        {
            current.Subscribe(count => _toggle.interactable = count > 0);
            _toggle.AddListener(OnToggle);
        }

        private void OnToggle(bool isOn)
        {
            if (_coroutineToggle != null)
                StopCoroutine(_coroutineToggle);

            if (isOn)
                _coroutineToggle = StartCoroutine(Enable_Cn());
            else
                _coroutineToggle = StartCoroutine(Disable_Cn());


            #region Local: Enable_Cn(), Disable_Cn()
            //=================================
            IEnumerator Enable_Cn()
            {
                for (int i = 0; i < _buttons.Count; i++)
                    yield return _buttons[i].Enable();

                _coroutineToggle = null;
            }
            //=================================
            IEnumerator Disable_Cn()
            {
                for (int i = _buttons.Count - 1; i >= 0; i--)
                    yield return _buttons[i].Disable();

                _coroutineToggle = null;
            }
            #endregion
        }

#if UNITY_EDITOR
        public override RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, ProjectColors colors)
        {
            _toggle.CheckmarkOn.color = colors.PanelBack;
            _toggle.CheckmarkOff.color = colors.PanelBack;

            RectTransform thisRectTransform = base.UpdateVisuals_Editor(pixelsPerUnit, padding, colors);

            Vector2 size = thisRectTransform.sizeDelta;
            _buttonPrefab.RectTransform.sizeDelta = new(size.x, size.x);
            ((RectTransform)_buttonContainer).anchoredPosition = new(0f, (size.x + size.y) * 0.5f + 20f);

            return thisRectTransform;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetComponent(ref _toggle);
            this.SetChildren(ref _buttonContainer, "ButtonContainer");
            EUtility.SetPrefab(ref _buttonPrefab);
        }
#endif
    }
}
