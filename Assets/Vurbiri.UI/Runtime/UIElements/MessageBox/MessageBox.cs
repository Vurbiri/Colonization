using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;

namespace Vurbiri.UI
{
	public class MessageBox : MonoBehaviour
    {
        [SerializeField] private Image _backImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private TextMeshProUGUI _hintTMP;
        [Header("Window")]
        [SerializeField] private Vector2 _maxSize;
        [SerializeField] private Vector2 _padding;
        [Header("Buttons")]
        
        [SerializeField] private RectTransform _buttonsRectTransform;
        [SerializeField] private Vector2 _buttonSize;
        [SerializeField] private Vector2 _buttonPadding;
        [SerializeField] private IdSet<MBButtonId, MBButton> _buttons;
        [Space]
        [SerializeField, Range(1f, 20f)] private float _fadeSpeed = 4f;

        private RectTransform _backTransform;
        private RectTransform _hintTransform;
        private Coroutine _coroutineShow, _coroutineHide;

        private IEnumerator Show_Cn(string text)
        {
            if (_coroutineHide != null)
            {
                StopCoroutine(_coroutineHide);
                _coroutineHide = null;
            }

            _hintTransform.sizeDelta = _maxSize;
            _hintTMP.text = text;
            _hintTMP.ForceMeshUpdate();

            Vector2 size = _hintTMP.textBounds.size;

            _hintTransform.sizeDelta = size;
            _backTransform.sizeDelta = size + _padding;

            yield return null;

            float alpha = _canvasGroup.alpha;
            while (alpha < 1f)
            {
                _canvasGroup.alpha = alpha += Time.unscaledDeltaTime * _fadeSpeed;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
            _coroutineShow = null;
        }

        private void Init()
        {
            _backTransform = _backImage.rectTransform;
            _hintTransform = _hintTMP.rectTransform;

            _hintTMP.enableWordWrapping = true;
            _hintTMP.overflowMode = TextOverflowModes.Overflow;

            _canvasGroup.alpha = 0f;
        }

        private void SetHint(string text)
        {
            _hintTransform.sizeDelta = _maxSize;
            _hintTMP.text = text;
            _hintTMP.ForceMeshUpdate();

            Vector2 size = _hintTMP.textBounds.size;

            _hintTransform.sizeDelta = size;
            _backTransform.sizeDelta = size + _padding;

            //_backTransform.ForceUpdateRectTransforms();
        }

#if UNITY_EDITOR

        public void UpdateVisuals_Editor(Color backColor, Color textColor)
        {
            //SetColors(backColor, textColor);

            _hintTMP.rectTransform.sizeDelta = _maxSize;
            _backImage.rectTransform.sizeDelta = _maxSize + _padding;
        }

        private void OnValidate()
        {
            this.SetChildren(ref _backImage);
            this.SetChildren(ref _hintTMP);
            //this.SetComponent(ref _canvasGroup);

            if (_canvasRectTransform == null)
                _canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }
#endif
    }
}
