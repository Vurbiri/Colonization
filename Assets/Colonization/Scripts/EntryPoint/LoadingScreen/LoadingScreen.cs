//Assets\Vurbiri\Runtime\EntryPoint\LoadingScreen\LoadingScreen.cs
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    sealed public class LoadingScreen : ASingleton<LoadingScreen>, ILoadingScreen
    {
        [SerializeField, Range(0.1f, 2f)] private float _speedSmooth = 0.5f;
        [Space]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private TMP_Text _descText;

        public string Description { set => _descText.text = value; }
        public float Progress { set { } }

        protected override void Awake()
        {
            _isNotDestroying = true;
            base.Awake();
            Turn(_instance == this);
        }

        public void Turn(bool isOn)
        {
            float alpha = isOn ? 1f : 0f;

            SetActive(isOn);
            _canvasGroup.alpha = alpha;
            _indicatorImage.canvasRenderer.SetAlpha(alpha);
            _descText.canvasRenderer.SetAlpha(alpha);
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

            CrossFadeAlpha(1f);
            _canvasGroup.alpha = 1f;
        }

        public IEnumerator SmoothOff()
        {
            CrossFadeAlpha(0f);
            _canvasGroup.blocksRaycasts = false;

            float alpha = _canvasGroup.alpha;
            while (alpha > 0f)
            {
                _canvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedSmooth;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private void CrossFadeAlpha(float alpha)
        {
            _indicatorImage.CrossFadeAlpha(alpha, 0.1f, true);
            _descText.CrossFadeAlpha(alpha, 0.1f, true);
        }

        private void SetActive(bool active)
        {
            _canvasGroup.blocksRaycasts = active;
            gameObject.SetActive(active);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            if (_descText == null)
                _descText = GetComponentInChildren<TMP_Text>();
            if (_indicatorImage == null)
                _indicatorImage = EUtility.GetComponentInChildren<Image>(this, "IndicatorImage");
        }
#endif
    }
}
