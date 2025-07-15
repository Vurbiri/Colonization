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

        private readonly Subscription<Id<MBButtonId>> _onClick = new();
        private RectTransform _thisRectTransform;
        private GameObject _thisObject;

        public Id<MBButtonId> Id => _id;

        public ISubscription<Id<MBButtonId>> Init()
        {
            _thisRectTransform = (RectTransform)transform;
            _thisObject = gameObject;
            _thisObject.SetActive(false);

            return _onClick;
        }

        public Unsubscription AddListener(Action<Id<MBButtonId>> action) => _onClick.Add(action);
        public void RemoveListener(Action<Id<MBButtonId>> action) => _onClick.Remove(action);
        
        public void Setup(Vector3 position)
        {
            _thisRectTransform.localPosition = position;
            _thisObject.SetActive(true);
        }


        public void SetActive(bool active) => _thisObject.SetActive(active);

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
