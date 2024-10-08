using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private float _speedSmooth = 1f;
        [Header("Индикатор"), Space]
        [SerializeField] private Image _indicatorSprite;
        [Space]
        [SerializeField] private Color _startColor = Color.white;
        [SerializeField] private Color _endColor = Color.gray;
        [SerializeField] private float _ratioSpeedFlashes = 1.5f;

        private Coroutine _coroutineSmooth, _coroutineIndicator;
        private CanvasGroup _thisCanvasGroup;
        private GameObject _self;

        public void Init(bool isOn)
        {
            _thisCanvasGroup = GetComponent<CanvasGroup>();
            _self = gameObject;

            if (isOn) On();
            else Off();

        }

        public void On()
        {
            _self.SetActive(true);
            _thisCanvasGroup.alpha = 1f;
            _coroutineIndicator ??= StartCoroutine(IndicatorFlashes_Coroutine());

        }
        public void Off()
        {
            _thisCanvasGroup.alpha = 0f;
            if (_coroutineIndicator != null)
            {
                StopCoroutine(_coroutineIndicator);
                _coroutineIndicator = null;
            }
            _self.SetActive(false);
        }

        public void SmoothOn()
        {
            _self.SetActive(true);
            if (_coroutineSmooth != null)
                StopCoroutine(_coroutineSmooth);

            _coroutineSmooth = StartCoroutine(SmoothOn_Coroutine());

            #region Local: SmoothOn_Coroutine()
            //=================================
            IEnumerator SmoothOn_Coroutine()
            {
                _coroutineIndicator ??= StartCoroutine(IndicatorFlashes_Coroutine());

                float alpha = _thisCanvasGroup.alpha;
                while (alpha < 1f)
                {
                    _thisCanvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedSmooth;
                    yield return null;
                }

                _thisCanvasGroup.alpha = 1f;
                _coroutineSmooth = null;
            }
            #endregion
        }

        public void SmoothOff()
        {
            if (_coroutineSmooth != null)
                StopCoroutine(_coroutineSmooth);

            _coroutineSmooth = StartCoroutine(SmoothOff_Coroutine());

            #region Local: SmoothOff_Coroutine()
            //=================================
            IEnumerator SmoothOff_Coroutine()
            {
                float alpha = _thisCanvasGroup.alpha;
                while (alpha > 0f)
                {
                    _thisCanvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedSmooth;
                    yield return null;
                }

                _thisCanvasGroup.alpha = 0f;

                if (_coroutineIndicator != null)
                    StopCoroutine(_coroutineIndicator);

                _coroutineIndicator = null;
                _coroutineSmooth = null;

                _self.SetActive(false);
            }
            #endregion
        }

        private IEnumerator IndicatorFlashes_Coroutine()
        {
            while (true)
            {
                _indicatorSprite.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time * _ratioSpeedFlashes, 1));
                yield return null;
            }
        }
    }
}