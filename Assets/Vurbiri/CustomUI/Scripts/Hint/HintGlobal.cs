using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Graphic), typeof(CanvasGroup))]
    public class HintGlobal : MonoBehaviour
    {
        [SerializeField] private TMP_Text _hint;
        [SerializeField, Range(0f, 5f)] private float _timeDelay = 1f;
        [SerializeField, Range(1f, 20f)] private float _fadeSpeed = 5f;
        [SerializeField] private float _maxWidth = 400f;
        [SerializeField] private Vector2 _padding = new(15f, 15f);

        private Language _localization;
        private CanvasGroup _thisCanvasGroup;
        private RectTransform _transformBack, _transformText;
        private Coroutine _coroutineShow, _coroutineHide;
        private Vector2 _size;

        private void Start()
        {
            _localization = Language.Instance;
            _thisCanvasGroup = GetComponent<CanvasGroup>();
            _transformBack = GetComponent<RectTransform>();
            _transformText = _hint.rectTransform;

            _hint.overflowMode = TextOverflowModes.Overflow;

            _thisCanvasGroup.alpha = 0f;
        }

        public bool Show(Files file, string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            SetHint(_localization.GetText(file, key));
            Show();

            return true;
        }

        public bool Show(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            SetHint(text);
            Show();

            return true;
        }

        private void Show()
        {
            _coroutineShow ??= StartCoroutine(Show_Coroutine());

            #region Local: Show_Coroutine()
            //=================================
            IEnumerator Show_Coroutine()
            {
                yield return new WaitForSecondsRealtime(_timeDelay);
                
                if (_coroutineHide != null)
                {
                    StopCoroutine(_coroutineHide);
                    _coroutineHide = null;
                }

                float alpha = _thisCanvasGroup.alpha;
                while (alpha < 1f)
                {
                    _thisCanvasGroup.alpha = alpha += Time.unscaledDeltaTime * _fadeSpeed;
                    yield return null;
                }

                _thisCanvasGroup.alpha = 1f;
                _coroutineShow = null;
            }
            #endregion
        }

        public bool Hide()
        {
            if (_coroutineShow != null)
            {
                StopCoroutine(_coroutineShow);
                _coroutineShow = null;
            }

            _coroutineHide ??= StartCoroutine(Hide_Coroutine());

            return true;

            #region Local: Hide_Coroutine()
            //=================================
            IEnumerator Hide_Coroutine()
            {
                float alpha = _thisCanvasGroup.alpha;
                while (alpha > 0f)
                {
                    _thisCanvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _fadeSpeed;
                    yield return null;
                }

                _thisCanvasGroup.alpha = 0f;
                _coroutineHide = null;
            }
            #endregion
        }

        private void SetHint(string text)
        {
            _hint.enableWordWrapping = false;
            _hint.text = text;
            _hint.ForceMeshUpdate();

            _size = _hint.textBounds.size;

            if (_size.x > _maxWidth)
            {
                _size.x = _maxWidth;
                _transformText.sizeDelta = _size;

                _hint.enableWordWrapping = true;
                _hint.ForceMeshUpdate();

                _size = _hint.textBounds.size;
            }

            _transformText.sizeDelta = _size;
            _transformBack.sizeDelta = _size + _padding;

            //_transformText.ForceUpdateRectTransforms();
            //_transformBack.ForceUpdateRectTransforms();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _hint ??= GetComponentInChildren<TMP_Text>();
        }

#endif
    }
}
