using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AVButtonBase : VSelectable, IPointerClickHandler, ISubmitHandler
    {
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

        protected internal abstract void Invoke();

        private bool Press()
        {
            if (IsActive() && IsInteractable())
            {
                UISystemProfilerApi.AddMarker("VButton.onClick", this);
                Invoke();
                return true;
            }

            return false;
        }
    }
}
