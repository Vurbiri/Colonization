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
        [SerializeField] private Vector2 _maxSize;
        [SerializeField] private Vector2 _padding;
        [Space]
        [SerializeField, MinMax(0f, 5f)] private WaitRealtime _timeDelay = 1.5f;
        [SerializeField] private CanvasGroupSwitcher _waitSwitch;

        protected RectTransform _backTransform, _hintTransform;
        private Coroutine _coroutineShow, _coroutineHide;
        
        public virtual void Init()
        {
            _waitSwitch.Disable();

            _backTransform = _backImage.rectTransform;
            _hintTransform = _hintTMP.rectTransform;

            _hintTMP.enableWordWrapping = true;
            _hintTMP.overflowMode = TextOverflowModes.Overflow;
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

            yield return _waitSwitch.Show();
            _coroutineShow = null;
        }

        private IEnumerator Hide_Cn()
        {
            yield return _waitSwitch.Hide();
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
            _waitSwitch.Alpha = 0f;
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

            if (_waitSwitch.CanvasGroup == null)
                _waitSwitch.OnValidate(GetComponentInChildren<CanvasGroup>());
        }
#endif
    }
}
