using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Graphic))]
    public class HintGlobal : MonoBehaviour
    {
        [SerializeField, GetComponentInChildren] private TMP_Text _hint;
        [SerializeField, Range(0f, 5f)] private float _timeDelay = 1f;
        [SerializeField, Range(1f, 20f)] private float _fadeSpeed = 5f;
        [SerializeField] private float _maxWidth = 400f;
        [SerializeField] private Vector2 _padding = new(15f, 15f);

        private Localization _localization;
        private Graphic _background;
        private RectTransform _transformBack, _transformText;
        private Coroutine _coroutineShow;
        private float _alfaBack, _alfaText;
        private float _fadeDurationBack, _fadeDurationText;
        private Vector2 _size;

        private void Start()
        {
            _localization = Localization.Instance;
            _background = GetComponent<Graphic>();
            _transformBack = GetComponent<RectTransform>();
            _transformText = _hint.rectTransform;

            _hint.overflowMode = TextOverflowModes.Overflow;

            _alfaBack = _background.color.a;
            _alfaText = _hint.color.a;

            _fadeDurationBack = _alfaBack / _fadeSpeed;
            _fadeDurationText = _alfaText / _fadeSpeed;

            AlphaReset();
        }

        public bool Show(TextFiles file, string key)
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
            AlphaReset();

            _coroutineShow ??= StartCoroutine(Show_Coroutine());

            #region Local: Show_Coroutine()
            //=================================
            IEnumerator Show_Coroutine()
            {
                yield return new WaitForSecondsRealtime(_timeDelay);
                _background.CrossFadeAlpha(_alfaBack, _fadeDurationBack, true);
                _hint.CrossFadeAlpha(_alfaText, _fadeDurationText, true);
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

            _background.CrossFadeAlpha(0f, _fadeDurationBack, true);
            _hint.CrossFadeAlpha(0f, _fadeDurationText, true);
            return true;
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

        private void AlphaReset()
        {
            _background.CrossFadeAlpha(0f, 0f, true);
            _hint.CrossFadeAlpha(0f, 0f, true);
        }
    }
}
