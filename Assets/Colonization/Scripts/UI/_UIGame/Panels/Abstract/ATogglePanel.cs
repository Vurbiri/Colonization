//Assets\Colonization\Scripts\UI\_UIGame\Panels\Abstract\ATogglePanel.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class ATogglePanel<T> : MonoBehaviour where T : APanelButton
    {
        [SerializeField] protected VToggle _toggle;
        [SerializeField] private CurrentMax _widget;
        [Space]
        [SerializeField] protected T _buttonPrefab;
        [SerializeField] protected Transform _buttonContainer;

        protected readonly List<T> _buttons = new();
        protected InputController _inputController;
        private Coroutine _coroutine;

        protected void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors colors)
        {
            current.Subscribe(count => _toggle.interactable = count > 0);
            _toggle.AddListener(OnToggle);

            _widget.Init(current, max, colors);
        }

        private void OnToggle(bool isOn)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (isOn)
                _coroutine = StartCoroutine(Enable_Cn());
            else
                _coroutine = StartCoroutine(Disable_Cn());


            #region Local: Enable_Cn(), Disable_Cn()
            //=================================
            IEnumerator Enable_Cn()
            {
                for (int i = 0; i < _buttons.Count; i++)
                    yield return _buttons[i].Enable();

                _coroutine = null;
            }
            //=================================
            IEnumerator Disable_Cn()
            {
                for (int i = _buttons.Count - 1; i >= 0; i--)
                    yield return _buttons[i].Disable();

                _coroutine = null;
            }
            #endregion
        }

#if UNITY_EDITOR
        public RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.BackgroundPanel;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _toggle.CheckmarkOn.color = colors.BackgroundPanel;
            _toggle.CheckmarkOff.color = colors.BackgroundPanel;

            Vector2 size = _widget.Size + padding * 2f;

            RectTransform thisRectTransform = (RectTransform)transform;

            thisRectTransform.sizeDelta = size;
            _buttonPrefab.RectTransform.sizeDelta = new(size.x, size.x);
            ((RectTransform)_buttonContainer).anchoredPosition = new(0f, (size.x + size.y) * 0.5f + 20f);

            return thisRectTransform;
        }

        protected virtual void OnValidate()
        {
            if (_toggle == null)
                _toggle = GetComponent<VToggle>();
            if (_widget == null)
                _widget = GetComponentInChildren<CurrentMax>();
            if (_buttonContainer == null)
                _buttonContainer = EUtility.GetComponentInChildren<Transform>(this, "ButtonContainer");

            EUtility.SetPrefab(ref _buttonPrefab);
        }
#endif
    }
}
