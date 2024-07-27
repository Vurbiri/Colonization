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
        [SerializeField, Range(0f, 1f)] private float _fadeDuration = 0.2f;
        [SerializeField] private float _maxWidth = 100f;
        [SerializeField] private float _padding = 1.1f;

        private Localization _localization;
        private Graphic _background;
        private RectTransform _transformBack, _transformText;
        private Coroutine _coroutineShow;
        private float _alfaBack, _alfaText;

        private void Start()
        {
            _localization = Localization.Instance;
            _background = GetComponent<Graphic>();
            _transformBack = GetComponent<RectTransform>();
            _transformText = _hint.rectTransform;

            _alfaBack = _background.color.a;
            _alfaText = _hint.color.a;

            _background.CrossFadeAlpha(0f, 0f, true);
            _hint.CrossFadeAlpha(0f, 0f, true);
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
            _coroutineShow ??= StartCoroutine(Show_Coroutine());

            #region Local: OnBack()
            //=================================
            IEnumerator Show_Coroutine()
            {
                yield return new WaitForSecondsRealtime(_timeDelay);
                _background.CrossFadeAlpha(_alfaBack, _fadeDuration, true);
                _hint.CrossFadeAlpha(_alfaText, _fadeDuration, true);
                _coroutineShow = null;
            }
            #endregion
        }

        public void Hide()
        {
            if (_coroutineShow != null)
            {
                StopCoroutine(_coroutineShow);
                _coroutineShow = null;
            }

            _background.CrossFadeAlpha(0f, _fadeDuration, true);
            _hint.CrossFadeAlpha(0f, _fadeDuration, true);
        }

        private void SetHint(string text)
        {
            bool active = gameObject.activeSelf;
            gameObject.SetActive(true);

            _hint.enableWordWrapping = false;

            _hint.text = text;
            _hint.ForceMeshUpdate();

            Vector2 size = _hint.textBounds.size;

            if (size.x > _maxWidth)
            {
                _transformText.sizeDelta = new(_maxWidth, size.y);

                _hint.enableWordWrapping = true;
                _hint.ForceMeshUpdate();

                size = _hint.textBounds.size;
            }
            _transformText.sizeDelta = size;
            _transformBack.sizeDelta = size + Vector2.one * _padding;

            gameObject.SetActive(active);
        }
    }
}
