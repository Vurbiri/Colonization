//Assets\Vurbiri\Runtime\CustomUI\Hint\HintGlobal.cs
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HintGlobal : MonoBehaviour
    {
        [SerializeField] private RectTransform _backTransform;
        [SerializeField] private TMP_Text _hint;
        [Space]
        [SerializeField, Range(0f, 5f)] private float _timeDelay = 1f;
        [SerializeField, Range(1f, 20f)] private float _fadeSpeed = 5f;
        [SerializeField] private float _maxWidth = 400f;
        [SerializeField] private Vector2 _padding = new(15f, 15f);

        private Language _localization;
        private CanvasGroup _thisCanvasGroup;
        private RectTransform _textTransform;
        private Coroutine _coroutineShow, _coroutineHide;
        private Vector2 _size;

        private void Start()
        {
            _localization = SceneServices.Get<Language>();
            _thisCanvasGroup = GetComponent<CanvasGroup>();
            _textTransform = _hint.rectTransform;

            _hint.color = SceneData.Get<HintTextColor>().ColorBase;
            _hint.overflowMode = TextOverflowModes.Overflow;

            _thisCanvasGroup.alpha = 0f;
        }

        public bool Show(Files file, string key, Vector3 position)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            
            _coroutineShow ??= StartCoroutine(Show_Coroutine(_localization.GetText(file, key), position));

            return true;
        }

        public bool Show(string text, Vector3 position)
        {
            if (string.IsNullOrEmpty(text) || !gameObject.activeInHierarchy)
                return false;

            if (_coroutineShow != null)
                StopCoroutine(_coroutineShow);

            _coroutineShow = StartCoroutine(Show_Coroutine(text, position));

            return true;
        }

        private IEnumerator Show_Coroutine(string text, Vector3 position)
        {
            yield return new WaitForSecondsRealtime(_timeDelay);

            if (_coroutineHide != null)
            {
                StopCoroutine(_coroutineHide);
                _coroutineHide = null;
            }

            position.y += SetHint(text);
            _backTransform.localPosition = position;

            float alpha = _thisCanvasGroup.alpha;
            while (alpha < 1f)
            {
                _thisCanvasGroup.alpha = alpha += Time.unscaledDeltaTime * _fadeSpeed;
                yield return null;
            }

            _thisCanvasGroup.alpha = 1f;
            _coroutineShow = null;
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

        private float SetHint(string text)
        {
            _hint.enableWordWrapping = false;
            _hint.text = text;
            _hint.ForceMeshUpdate();

            _size = _hint.textBounds.size;

            if (_size.x > _maxWidth)
            {
                _size.x = _maxWidth;
                _backTransform.sizeDelta = _size;

                _hint.enableWordWrapping = true;
                _hint.ForceMeshUpdate();

                _size = _hint.textBounds.size;
            }

            _textTransform.sizeDelta = _size;
            _backTransform.sizeDelta = _size + _padding;

            return _backTransform.sizeDelta.y / 2f;

            //_transformText.ForceUpdateRectTransforms();
            //_transformBack.ForceUpdateRectTransforms();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_backTransform == null)
                _backTransform = GetComponentInChildren<Image>().rectTransform;

            if (_hint == null)
                _hint = GetComponentInChildren<TMP_Text>();
        }
#endif
    }
}
