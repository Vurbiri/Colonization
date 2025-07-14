using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Reactive;
namespace Vurbiri.UI
{
	sealed public class MBButton : VSelectable, IValueId<MBButtonId>, IPointerClickHandler, ISubmitHandler
    {
		[SerializeField] private Id<MBButtonId> _id;

        private readonly Subscription<int> _onClick = new();

        public Id<MBButtonId> Id => _id;

        public void Init(Vector2 size)
        {
            var rectTransform = (RectTransform)transform;
            rectTransform.sizeDelta = size;
        }

        public Unsubscription AddListener(Action<int> action) => _onClick.Add(action);
        public void RemoveListener(Action<int> action) => _onClick.Remove(action);

        private bool Press()
        {
            if (IsActive() && IsInteractable())
            {
                UISystemProfilerApi.AddMarker("VButton.onClick", this);
                _onClick.Invoke(_id);
                return true;
            }

            return false;
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
            float fadeTime = colors.fadeDuration;

            while (fadeTime > 0f)
            {
                fadeTime -= Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}
