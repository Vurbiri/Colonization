//Assets\Vurbiri.UI\Runtime\VButton.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [AddComponentMenu("UI Vurbiri/Button", 30)]
    sealed public class VButton : VSelectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField]
        private UnitySubscriber _onClick = new();
        public Subscriber OnClick => _onClick;

        private VButton() { }

        protected override void Start()
        {
            base.Start();

            _onClick.Clear();
        }

        public Unsubscriber AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("VButton.onClick", this);
            _onClick.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
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
