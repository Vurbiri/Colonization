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
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private Vector2 _maxSize;
        [SerializeField] private Vector2 _padding;
        [Space]
        [SerializeField, MinMax(0f, 5f)] private WaitRealtime _timeDelay = 1.5f;
        [SerializeField] private WaitSwitchFloat _waitSwitch;

        protected RectTransform _backTransform;
        protected RectTransform _hintTransform;
        private Coroutine _coroutineShow, _coroutineHide;
        
        public virtual void Init()
        {
            _canvasGroup.alpha = 0f;

            _backTransform = _backImage.rectTransform;
            _hintTransform = _hintTMP.rectTransform;

            _hintTMP.enableWordWrapping = true;
            _hintTMP.overflowMode = TextOverflowModes.Overflow;

            _waitSwitch.Init();
        }

        public bool Show(string text, Vector3 position, Vector3 offset)
        {
            bool result;
            if (result = !string.IsNullOrEmpty(text) & gameObject.activeInHierarchy)
            {
                StopCoroutine(ref _coroutineShow);
                _coroutineShow = StartCoroutine(Show_Cn(text, position, offset));
            }
            return result;
        }
        public bool Hide()
        {
            StopCoroutine(ref _coroutineShow);
            _coroutineHide ??= StartCoroutine(Hide_Cn());

            return true;
        }

        public void SetColors(Color backColor, Color textColor)
        {
            _backImage.color = backColor;
            _hintTMP.color = textColor;
        }

        protected abstract void SetPosition(Vector3 position, Vector3 offset);

        private IEnumerator Show_Cn(string text, Vector3 position, Vector3 offset)
        {
            yield return _timeDelay.Restart();

            StopCoroutine(ref _coroutineHide);

            _hintTransform.sizeDelta = _maxSize;
            _hintTMP.text = text;
            _hintTMP.ForceMeshUpdate();

            Vector2 size = _hintTMP.textBounds.size;

            _hintTransform.sizeDelta = size;
            _backTransform.sizeDelta = size + _padding;

            yield return null;
            SetPosition(position, offset);

            yield return _waitSwitch.Forward(_canvasGroup.alpha);
            _coroutineShow = null;
        }

        private IEnumerator Hide_Cn()
        {
            yield return _waitSwitch.Backward(_canvasGroup.alpha);
            _coroutineHide = null;
        }

        private void StopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private void OnDisable()
        {
            _canvasGroup.alpha = 0f;
            _coroutineHide = null;
            _coroutineShow = null;
        }

#if UNITY_EDITOR
        public virtual void UpdateVisuals_Editor(Color backColor, Color textColor)
        {
            SetColors(backColor, textColor);

            _hintTMP.rectTransform.sizeDelta = _maxSize;
            _backImage.rectTransform.sizeDelta = _maxSize + _padding;
        }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _backImage);
            this.SetChildren(ref _hintTMP);
            this.SetComponent(ref _canvasGroup);

            if(!_waitSwitch.IsValid_Editor)
                _waitSwitch.OnValidate(0f, 1f, _canvasGroup.GetSetor<float>(nameof(_canvasGroup.alpha)));
        }
#endif
    }
}
