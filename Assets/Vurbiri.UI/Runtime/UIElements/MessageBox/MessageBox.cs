using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MessageBox : MonoBehaviour
    {
        private static MessageBox _instance;

        [SerializeField] private CanvasGroupSwitcher _waitSwitch;
        [Header("Window")]
        [SerializeField] private Image _windowImage;
        [SerializeField] private Vector2 _windowPadding;
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _textTMP;
        [SerializeField] private Vector2 _textMaxSize;
        [SerializeField] private Vector2 _textPadding;
        [Header("Buttons")]
        [SerializeField] private RectTransform _buttonsRectTransform;
        [SerializeField] private Vector2 _buttonBounds;
        [SerializeField] private IdSet<MBButtonId, MBButton> _buttons;

        private readonly Subscription<Id<MBButtonId>> _onClick = new();
        private readonly WaitResultSource<Id<MBButtonId>> _waitResult = new();

        private RectTransform _windowRectTransform;
        private RectTransform _textRectTransform;
        private Coroutine _coroutineShow, _coroutineHide;

        private Id<MBButtonId>[] _currentIds = new Id<MBButtonId>[0];

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                _waitSwitch.Init();

                _windowRectTransform = _windowImage.rectTransform;
                _textRectTransform = _textTMP.rectTransform;

                _textTMP.enableWordWrapping = true;
                _textTMP.overflowMode = TextOverflowModes.Overflow;

                for (int i = 0; i < MBButtonId.Count; i++)
                    _buttons[i].Init().Add(OnClick);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static WaitResult<Id<MBButtonId>> Show(string text, params Id<MBButtonId>[] buttonIds) => _instance.Setup(text, buttonIds);

        private WaitResult<Id<MBButtonId>> Setup(string text, params Id<MBButtonId>[] buttonIds)
        {
            for (int i = _currentIds.Length - 1; i >= 0; i--)
                _buttons[_currentIds[i]].SetActive(false);

            _currentIds = buttonIds;

            int count = buttonIds.Length;
            float start = (1 - count) * 0.5f * _buttonBounds.x;
            for (int i = 0; i < count; i++)
                _buttons[buttonIds[i]].Setup(new Vector3(start + _buttonBounds.x * i, 0f, 0f));

            Vector2 buttonsSize = new(_buttonBounds.x * count, _buttonBounds.y);
            _buttonsRectTransform.sizeDelta = buttonsSize;

            _textRectTransform.sizeDelta = _textMaxSize;
            _textTMP.text = text;
            _textTMP.ForceMeshUpdate();

            Vector2 textSize = _textTMP.textBounds.size;
            _textRectTransform.sizeDelta = (textSize += _textPadding);

            _buttonsRectTransform.localPosition = new(0f, -textSize.y * 0.5f, 0f);
            _textRectTransform.localPosition = new(0f, buttonsSize.y * 0.5f, 0f);

            _windowRectTransform.sizeDelta = new(Mathf.Max(buttonsSize.x, textSize.x) + _windowPadding.x, buttonsSize.y + textSize.y + _windowPadding.y);

            StopCoroutine(ref _coroutineHide);
            _coroutineShow ??= StartCoroutine(Show_Cn());

            return _waitResult.Restart();

            #region Local: Show_Cn()
            //=================================
            IEnumerator Show_Cn()
            {
                yield return _waitSwitch.Show();

                _coroutineShow = null;
            }
            #endregion
        }

        private void OnClick(Id<MBButtonId> id)
        {
            _waitResult.SetResult(id);
            _onClick.Invoke(id);
        }
        

        private void StopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            this.SetChildren(ref _windowImage);
            this.SetChildren(ref _textTMP);

            _waitSwitch.OnValidate(this);
        }
#endif
    }
}
