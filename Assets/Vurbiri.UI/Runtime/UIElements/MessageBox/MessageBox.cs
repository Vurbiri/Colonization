using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;

namespace Vurbiri.UI
{
    public class MessageBox : MonoBehaviour
    {
        private static MessageBox s_instance;
        private static readonly Queue<Task> s_tasks = new();
        
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
        
        private RectTransform _windowRectTransform;
        private RectTransform _textRectTransform;
        private Coroutine _coroutineShow, _coroutineHide;

        private WaitButtonSource _currentWait;
        private Id<MBButtonId>[] _currentIds = new Id<MBButtonId>[0];

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);

                _waitSwitch.Disable();

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

        public static void SetColors(Color windowColor, Color textColor)
        {
            s_instance._windowImage.color = windowColor;
            s_instance._textTMP.color = textColor;
        }

        public static WaitButton Open(string text, params Id<MBButtonId>[] buttonIds)
        {
            Throw.IfLengthZero(buttonIds, "buttonIds");
            
            if (s_instance._currentWait != null)
                return new Task(text, buttonIds).waitResult;
            else
                return s_instance.Setup(text, buttonIds, new());
        }

        internal static void Abort(WaitButton wait)
        {
            if (s_instance._currentWait == wait)
                s_instance.Hide();
        }

        private WaitButton Setup(string text, Id<MBButtonId>[] buttonIds, WaitButtonSource waitResult)
        {
            for (int i = _currentIds.Length - 1; i >= 0; i--)
                _buttons[_currentIds[i]].Deactivate();

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

            _coroutineShow ??= StartCoroutine(Show_Cn());

            return _currentWait = waitResult;

            #region Local: Show_Cn()
            //--------------------------------
            IEnumerator Show_Cn()
            {
                yield return _coroutineHide;
                yield return _waitSwitch.Show();
                _coroutineShow = null;
            }
            //--------------------------------
            #endregion
        }

        private void Hide()
        {
            _currentWait = null;

            if (_coroutineShow != null)
            {
                StopCoroutine(_coroutineShow);
                _coroutineShow = null;
            }

            _coroutineHide ??= StartCoroutine(Hide_Cn());

            #region Local: Hide_Cn()
            //--------------------------------
            IEnumerator Hide_Cn()
            {
                yield return _waitSwitch.Hide();
                _coroutineHide = null;
                
                if (s_tasks.Count > 0)
                    s_tasks.Dequeue().Run(this);
            }
            //--------------------------------
            #endregion
        }

        private void OnClick(Id<MBButtonId> id)
        {
            _currentWait.SetResult(id);

            Hide();
        }

        private void OnDestroy()
        {
            if (s_instance == this)
                s_instance = null;
        }

        #region Nested: Task()
        //*************************************************************************
        private class Task
        {
            public readonly string text;
            public readonly Id<MBButtonId>[] buttonIds;
            public readonly WaitButtonSource waitResult;

            public Task(string text, Id<MBButtonId>[] buttonIds)
            {
                this.text = text;
                this.buttonIds = buttonIds;
                this.waitResult = new();

                s_tasks.Enqueue(this);
            }

            public void Run(MessageBox parent)
            {
                if (waitResult.IsWait)
                    parent.Setup(text, buttonIds, waitResult);
                else if (s_tasks.Count > 0)
                    s_tasks.Dequeue().Run(parent);
            }
        }
        //*************************************************************************
        #endregion

#if UNITY_EDITOR

        private void OnValidate()
        {
            this.SetChildren(ref _windowImage);
            this.SetChildren(ref _textTMP);

            if(_waitSwitch.CanvasGroup == null)
                _waitSwitch.OnValidate(GetComponentInChildren<CanvasGroup>());
        }
#endif
    }
}
