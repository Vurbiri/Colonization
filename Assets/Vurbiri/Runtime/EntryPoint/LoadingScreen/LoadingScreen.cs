//Assets\Vurbiri\Runtime\EntryPoint\LoadingScreen\LoadingScreen.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)] private float _speedSmooth = 0.5f;
        [SerializeField, Range(0f, 0.5f)] private float _alphaThreshold = 0.1f;
        [Space]
        [SerializeField] private GameObject _indicatorImage;

        private Coroutine _coroutineSmooth;
        private CanvasGroup _thisCanvasGroup;
        private GameObject _self;
        private bool _isOn;

        private const string PATH_TO_LOADING_SCREEN = "LoadingScreenCanvas";

        public static LoadingScreen Create()
        {
            LoadingScreen loadingScreen = Resources.Load<LoadingScreen>(PATH_TO_LOADING_SCREEN);
            return Instantiate(loadingScreen).Init();
        }

        private LoadingScreen Init()
        {
            DontDestroyOnLoad(gameObject);
            
            _thisCanvasGroup = GetComponent<CanvasGroup>();
            _self = gameObject;
            _isOn = _self.activeSelf;

            TurnOnOf(true);

            return this;
        }

        public void TurnOnOf(bool isOn)
        {
            _isOn = isOn;

            _thisCanvasGroup.alpha = isOn ? 1f : 0f;
            _indicatorImage.SetActive(isOn);
            _self.SetActive(isOn);
        }

        public WaitActivate SmoothOn_Wait()
        {
            WaitActivate wait = new();

            if(_isOn)
            {
                wait.Activate();
                return wait;
            }

            _isOn = true;
            _self.SetActive(true);
            if (_coroutineSmooth != null)
                StopCoroutine(_coroutineSmooth);

            _coroutineSmooth = StartCoroutine(SmoothOn_Cn());

            return wait;

            #region Local: SmoothOn_Cn()
            //=================================
            IEnumerator SmoothOn_Cn()
            {

                float alpha = _thisCanvasGroup.alpha;
                while (alpha < 0.98f)
                {
                    _thisCanvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedSmooth;
                    yield return null;
                }

                _indicatorImage.SetActive(true);
                _thisCanvasGroup.alpha = 1f;
                _coroutineSmooth = null;
                wait.Activate();
            }
            #endregion
        }

        public WaitActivate SmoothOff_Wait()
        {
            WaitActivate wait = new();

            if (!_isOn)
            {
                wait.Activate();
                return wait;
            }

            _isOn = false;
            _indicatorImage.SetActive(false);
            if (_coroutineSmooth != null)
                StopCoroutine(_coroutineSmooth);

            _coroutineSmooth = StartCoroutine(SmoothOff_Cn());

            return wait;

            #region Local: SmoothOff_Cn()
            //=================================
            IEnumerator SmoothOff_Cn()
            {
                float alpha = _thisCanvasGroup.alpha;
                while (alpha > _alphaThreshold)
                {
                    _thisCanvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedSmooth;
                    yield return null;
                }

                _thisCanvasGroup.alpha = 0f;
                _coroutineSmooth = null;

                wait.Activate();
                _self.SetActive(false);
            }
            #endregion
        }
    }
}
