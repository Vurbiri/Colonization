using System.Collections;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AWorldMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private float _speedOpen = 8f;
        [SerializeField] private float _speedClose = 10f;

        private CanvasGroup _thisCanvasGroup;
        private Coroutine _coroutine;
        private float _targetAlpha;

        protected readonly Subscription<IMenu, bool> _eventActive = new();


        private void Awake()
        {
            _thisCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void Open()
        {
            if (PreEnable())
                _coroutine = StartCoroutine(Open_Cn());

            #region Local: Open_Cn()
            //=================================
            IEnumerator Open_Cn()
            {
                float alpha = _thisCanvasGroup.alpha;
                while (alpha < 1f)
                {
                    _thisCanvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedOpen;
                    yield return null;
                }

                Enable();
            }
            #endregion
        }
        public void Close()
        {
            if(PreDisable())
                _coroutine = StartCoroutine(Close_Cn());

            #region Local: Close_Cn()
            //=================================
            IEnumerator Close_Cn()
            {
                float alpha = _thisCanvasGroup.alpha;
                while (alpha > 0f)
                {
                    _thisCanvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedClose;
                    yield return null;
                }

                Disable();
            }
            #endregion
        }

        public void OpenInstant()
        {
            if (PreEnable())
                Enable();
        }

        public void CloseInstant()
        {
            if (PreDisable())
                Disable();
        }

        private bool PreEnable()
        {
            if (_thisCanvasGroup.blocksRaycasts)
                return false;
            
            _targetAlpha = 1f;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _eventActive.Invoke(this, true);
            return true;
        }
        protected virtual void Enable()
        {
            _thisCanvasGroup.alpha = 1f;
            _thisCanvasGroup.blocksRaycasts = true;
            _coroutine = null;
        }

        private bool PreDisable()
        {
            if (!_thisCanvasGroup.blocksRaycasts)
                return false;

            _targetAlpha = 0f;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _thisCanvasGroup.blocksRaycasts = false;
            return true;
        }
        protected virtual void Disable()
        {
            _thisCanvasGroup.alpha = 0f;
            _coroutine = null;
            _eventActive.Invoke(this, false);
        }

        private void OnEnable()
        {
            bool isEnabled = _targetAlpha > 0.1f;

            _thisCanvasGroup.alpha = _targetAlpha;
            _thisCanvasGroup.blocksRaycasts = isEnabled;

            if (isEnabled)
                _eventActive.Invoke(this, true);
        }

        private void OnDisable()
        {
            _coroutine = null;
            if (_targetAlpha > 0.1f)
                _eventActive.Invoke(this, false);
        }

#if UNITY_EDITOR

        public virtual void SetButtonPosition(float buttonDistance)
        {
        }
#endif
    }
}
