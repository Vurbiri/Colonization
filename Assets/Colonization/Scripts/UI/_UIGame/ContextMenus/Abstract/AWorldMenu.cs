//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Abstract\AWorldMenu.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AWorldMenu : MonoBehaviour
    {
        [SerializeField] private float _speedOpen = 8f;
        [SerializeField] private float _speedClose = 10f;

        private CanvasGroup _thisCanvasGroup;
        private GameObject _thisGameObject;
        private Coroutine _coroutine;
        private float _targetAlpha;

        protected Crossroad _currentCrossroad;
        protected readonly Signer<GameObject, bool> _eventActive = new();

        private void Awake()
        {
            _thisCanvasGroup = GetComponent<CanvasGroup>();
            _thisGameObject = gameObject;
        }

        public void Open()
        {
            PreEnable();
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
            PreDisable();
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
            PreEnable();
            Enable();
        }

        public void CloseInstant()
        {
            PreDisable();
            Disable();
        }

        protected virtual void OnClose()
        {
            Close();
        }

        private void PreEnable()
        {
            _targetAlpha = 1f;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _eventActive.Invoke(_thisGameObject, true);
        }
        private void Enable()
        {
            _thisCanvasGroup.alpha = 1f;
            _thisCanvasGroup.blocksRaycasts = true;
            _coroutine = null;
        }

        private void PreDisable()
        {
            _targetAlpha = 0f;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _thisCanvasGroup.blocksRaycasts = false;
        }
        private void Disable()
        {
            _thisCanvasGroup.alpha = 0f;
            _coroutine = null;
            _eventActive.Invoke(_thisGameObject, false);
        }

        private void OnEnable()
        {
            bool isEnabled = _targetAlpha > 0.1f;

            _thisCanvasGroup.alpha = _targetAlpha;
            _thisCanvasGroup.blocksRaycasts = isEnabled;

            if (isEnabled)
                _eventActive.Invoke(_thisGameObject, true);
        }

        private void OnDisable()
        {
            _coroutine = null;
            if (_targetAlpha > 0.1f)
                _eventActive.Invoke(_thisGameObject, false);
        }
    }
}
