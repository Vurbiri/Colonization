//Assets\Vurbiri.UI\Runtime\VButton.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Button", 30)]
    sealed public class VButton : VSelectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] private UnitySigner _onClick = new();

        private VButton() { }

        protected override void Start()
        {
            base.Start();

            _onClick.Clear();
            _onClick.Add(ProfilerApiAddMarker);
        }

        public Unsubscriber AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        private bool Press()
        {
            if (!IsActive() || !IsInteractable())
                return false;

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

        private void ProfilerApiAddMarker() => UISystemProfilerApi.AddMarker("VButton.onClick", this);
    }
}
