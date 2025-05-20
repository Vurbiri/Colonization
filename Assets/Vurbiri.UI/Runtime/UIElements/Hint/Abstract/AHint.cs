//Assets\Vurbiri.UI\Runtime\UIElements\Hint\Abstract\AHint.cs
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AHint : MonoBehaviour
    {
        [SerializeField] private Image _backImage;
        [SerializeField] private TextMeshProUGUI _hintTMP;
        [Space]
        [SerializeField, MinMax(0f, 5f)] private WaitRealtime _timeDelay = 1.5f;
        [SerializeField, Range(1f, 20f)] private float _fadeSpeed = 4f;
        [SerializeField] private Vector2 _padding;

        protected RectTransform _backTransform;
        private RectTransform _hintTransform;
        private CanvasGroup _thisCanvasGroup;
        private Coroutine _coroutineShow, _coroutineHide;
        private Vector2 _defaultSize;
        
        public virtual void Init(Color backColor, Color textColor)
        {
            _backTransform = _backImage.rectTransform;
            _hintTransform = _hintTMP.rectTransform;
            _thisCanvasGroup = GetComponent<CanvasGroup>();
            _defaultSize = _hintTransform.sizeDelta;

            _hintTMP.enableWordWrapping = true;

            _backImage.color = backColor;

            _hintTMP.color = textColor;
            _hintTMP.overflowMode = TextOverflowModes.Overflow;

            _thisCanvasGroup.alpha = 0f;
            _backImage = null;
        }

        public bool Show(string text, Vector3 position, Vector3 offset)
        {
            bool result;
            if (result = !string.IsNullOrEmpty(text) & gameObject.activeInHierarchy)
            {
                if (_coroutineShow != null)
                    StopCoroutine(_coroutineShow);

                _coroutineShow = StartCoroutine(Show_Cn(text, position, offset));
            }
            return result;
        }


        public bool Hide()
        {
            if (_coroutineShow != null)
            {
                StopCoroutine(_coroutineShow);
                _coroutineShow = null;
            }

            _coroutineHide ??= StartCoroutine(Hide_Cn());

            return true;
        }

        protected abstract void SetPosition(Vector3 position, Vector3 offset);

        private IEnumerator Show_Cn(string text, Vector3 position, Vector3 offset)
        {
            yield return _timeDelay.Restart();

            if (_coroutineHide != null)
            {
                StopCoroutine(_coroutineHide);
                _coroutineHide = null;
            }

            SetHint(text);
            yield return null;
            SetPosition(position, offset);

            float alpha = _thisCanvasGroup.alpha;
            while (alpha < 1f)
            {
                _thisCanvasGroup.alpha = alpha += Time.unscaledDeltaTime * _fadeSpeed;
                yield return null;
            }

            _thisCanvasGroup.alpha = 1f;
            _coroutineShow = null;
        }

        private void SetHint(string text)
        {
            _hintTransform.sizeDelta = _defaultSize;
            _hintTMP.text = text;
            _hintTMP.ForceMeshUpdate();

            Vector2 size = _hintTMP.textBounds.size;

            _hintTransform.sizeDelta = size;
            _backTransform.sizeDelta = size + _padding;

            //_backTransform.ForceUpdateRectTransforms();
        }

        private IEnumerator Hide_Cn()
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

        private void OnDisable()
        {
            _thisCanvasGroup.alpha = 0f;
            _coroutineHide = null;
            _coroutineShow = null;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;

            if (_backImage == null)
                _backImage = GetComponent<Image>();
            if (_hintTMP == null)
                _hintTMP = GetComponentInChildren<TextMeshProUGUI>();

            Rebuild();
        }

        public async void Rebuild()
        {
           await System.Threading.Tasks.Task.Delay(2);
           if (!Application.isPlaying)
              _backImage.rectTransform.sizeDelta = _hintTMP.rectTransform.sizeDelta + _padding;
        }
#endif
    }
}
