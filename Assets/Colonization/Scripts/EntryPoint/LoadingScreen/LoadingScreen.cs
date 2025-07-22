using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class LoadingScreen : AClosedSingleton<LoadingScreen>, ILoadingScreen
    {
        [SerializeField, Range(0.1f, 2f)] private float _speedSmooth = 0.5f;
        [Space]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _fillBar;
        [SerializeField] private TextMeshProUGUI _descText;
        [SerializeField] private Graphic _indicator;

        private Graphic[] _graphics;

        private DrivenRectTransformTracker _tracker;

        public string Description { set => _descText.text = value; }
        public float Progress { set => _fillBar.anchorMin = new(Mathf.Clamp01(value), 0f); }

        protected override void Awake()
        {
            base.Awake();
            if (s_instance == this)
            {
                _graphics = new Graphic[] { _descText, _indicator, _fillBar.parent.GetComponent<Graphic>() };
                _indicator = null;

                SetActive(true);
                _canvasGroup.alpha = 1f;
            }
        }

        public IEnumerator SmoothOn()
        {
            SetActive(true);

            float alpha = _canvasGroup.alpha;
            while (alpha < 1)
            {
                _canvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedSmooth;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
            CrossFadeAlpha(1f);
        }

        public IEnumerator SmoothOff()
        {
            _canvasGroup.blocksRaycasts = false;
            CrossFadeAlpha(0f);

            float alpha = _canvasGroup.alpha;
            while (alpha > 0f)
            {
                _canvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedSmooth;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private void SetActive(bool active)
        {
            _canvasGroup.blocksRaycasts = active;
            gameObject.SetActive(active);
        }

        private void CrossFadeAlpha(float alpha)
        {
            for(int i = _graphics.Length - 1; i >= 0; i--)
            {
                _graphics[i].CrossFadeAlpha(alpha, 0.1f, true);
            }
        }

        private void OnEnable()
        {
            _tracker.Add(this, _fillBar, DrivenTransformProperties.Anchors);

            _fillBar.anchorMin = Vector2.zero;
            _fillBar.anchorMax = Vector2.one;
        }
        private void OnDisable()
        {
            _tracker.Clear();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetComponent(ref _canvasGroup);

            this.SetChildren(ref _descText);
            this.SetChildren(ref _fillBar, "Value");
            this.SetChildren(ref _indicator, "Indicator");
        }
#endif
    }
}
