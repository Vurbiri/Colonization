using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.EntryPoint;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class LoadingScreen : AClosedSingleton<LoadingScreen>, ILoadingScreen
    {
        [SerializeField, MinMax(1f, 3f)] private WaitRealtime _smooth = 2f;
        [SerializeField, Range(0.1f, 2f)] private float _indicatorSpeed = 0.5f;
        [Space]
        [SerializeField] private Image _background, _indicator;
        [SerializeField] private TextMeshProUGUI _descText;
        [SerializeField] private Graphic _bar, _value;

        private RectTransform _fillBar;
        private Graphic[] _graphics;

        private DrivenRectTransformTracker _tracker;

        public string Description { set => _descText.text = value; }
        public float Progress { set => _fillBar.anchorMin = new(Mathf.Clamp01(value), 0f); }

        protected override void Awake()
        {
            base.Awake();
            if (s_instance == this)
            {
                _fillBar = _value.rectTransform;
                _graphics = new Graphic[] { _indicator, _descText, _bar, _value };
                _bar = null; _value = null;

                SetActive(true);
            }
        }

        public IEnumerator SmoothOn()
        {
            SetActive(true);

            _background.CrossFadeAlpha(1f, _smooth.Time, true);
            yield return _smooth.Restart();

            CrossFadeAlpha(1f);
        }

        public IEnumerator SmoothOff()
        {
            _background.raycastTarget = false;
            CrossFadeAlpha(0f);

            _background.CrossFadeAlpha(0f, _smooth.Time, true);
            yield return _smooth.Restart();

            gameObject.SetActive(false);
        }

        [Impl(256)]
        private void SetActive(bool active)
        {
            _background.raycastTarget = active;
            gameObject.SetActive(active);
        }

        [Impl(256)]
        private void CrossFadeAlpha(float alpha)
        {
            for(int i = _graphics.Length - 1; i >= 0; --i)
                _graphics[i].CrossFadeAlpha(alpha, 0.1f, true);
        }

        private IEnumerator IndicatorTurn_Cn()
        {
            float start = 0f, end = 1f, progress, sign;
            _indicator.fillClockwise = true;

            while (true)
            {
                progress = 0f; sign = end - start;

                while (progress < 1f)
                {
                    progress += Time.unscaledDeltaTime * _indicatorSpeed;
                    _indicator.fillAmount = start + sign * progress;
                    yield return null;
                }
               
                (start, end) = (end, start);
                _indicator.fillClockwise = !_indicator.fillClockwise;
            }
        }

        private void OnEnable()
        {
            _tracker.Add(this, _fillBar, DrivenTransformProperties.Anchors);

            _fillBar.anchorMin = Vector2.zero;
            _fillBar.anchorMax = Vector2.one;

            StartCoroutine(IndicatorTurn_Cn());
        }
        private void OnDisable()
        {
            _tracker.Clear();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetChildren(ref _background, "Background");
            this.SetChildren(ref _indicator, "Indicator");
            this.SetChildren(ref _descText);
            this.SetChildren(ref _bar, "Bar");
            this.SetChildren(ref _value, "Value");
        }
#endif
    }
}
