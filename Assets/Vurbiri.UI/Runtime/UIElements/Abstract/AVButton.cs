using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public abstract class AVButton : VSelectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] protected UniSubscription _onClick = new();

        protected override void Start()
        {
            base.Start();

            _onClick.Init();
        }

        public Unsubscription AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

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
                StartCoroutine(OnFinishSubmit_Cn());
            }

            #region Local: OnFinishSubmit_Cn()
            //=================================
            IEnumerator OnFinishSubmit_Cn()
            {
                float fadeTime = colors.fadeDuration;

                while (fadeTime > 0f)
                {
                    fadeTime -= Time.unscaledDeltaTime;
                    yield return null;
                }

                DoStateTransition(currentSelectionState, false);
            }
            #endregion
        }


        private bool Press()
        {
            if (IsActive() && IsInteractable())
            {
                UISystemProfilerApi.AddMarker("VButton.onClick", this);
                _onClick.Invoke();
                return true;
            }

            return false;
        }
    }
}
