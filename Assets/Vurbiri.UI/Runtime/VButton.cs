//Assets\Vurbiri.UI\Runtime\VButton.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Button", 30)]
    public class VButton : VSelectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] protected UniSigner _onClick = new();

        protected VButton() { }

        protected override void Start()
        {
            base.Start();

            _onClick.Init();
        }

        public Unsubscriber AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        private bool Press()
        {
            if (!IsActive() || !IsInteractable())
                return false;

            UISystemProfilerApi.AddMarker("VButton.onClick", this);
            _onClick.Invoke();
            return true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Press();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (Press())
            {
                DoStateTransition(SelectionState.Pressed, false);
                StartCoroutine(OnFinishSubmit());
            }
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}
