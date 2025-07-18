using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    internal class Banner : MonoBehaviour
	{
        private static Vector2 s_maxSize, s_padding, s_direct;

        [SerializeField] private Image _windowImage;
        [SerializeField] private TextMeshProUGUI _textTMP;
        [Space]
        [SerializeField] private CanvasGroupSwitcher _waitSwitch;

        private MoveUsingLerp _move;
        private RectTransform _windowTransform, _textTransform;
        private Vector3 _directSize;
        private Coroutine _coroutine;
        private int _index;
        private Action _endWork;

        public static void Init(Vector2 maxSize, Vector2 padding, Vector2 direct)
        {
            s_maxSize = maxSize; s_padding = padding; s_direct = direct;
        }

        public void Init(float moveSpeed, Action onEndWork)
        {
            _windowTransform = _windowImage.rectTransform;
            _textTransform = _textTMP.rectTransform;

            _textTMP.enableWordWrapping = true;
            _textTMP.overflowMode = TextOverflowModes.Overflow;

            _waitSwitch.Disable();
            _move = new(_windowTransform, moveSpeed);
            _endWork = onEndWork;
        }

        public Vector3 Setup(int index, string text, Color textColor, Vector3 currentPosition, IEnumerator delay, bool oneScene = true)
        {
            _index = index;

            _textTransform.sizeDelta = s_maxSize;
            _textTMP.text = text;
            _textTMP.color = textColor;
            _textTMP.ForceMeshUpdate();

            Vector2 size = _textTMP.textBounds.size;

            _textTransform.sizeDelta = size;
            _windowTransform.sizeDelta = size += s_padding;

            _directSize = new(size.x * s_direct.x, size.y * s_direct.y, 0f);
            _windowTransform.localPosition = new(currentPosition.x * _directSize.x * 0.5f, currentPosition.y * _directSize.y * 0.5f, 0f);

            _coroutine = StartCoroutine(Work_Cn(delay));
            if (oneScene) SceneManager.sceneUnloaded += OnSceneUnloaded;

            return currentPosition + _directSize;
        }

        public Vector3 SetPosition(int index, Vector3 currentPosition)
        {
            if (_index != index)
            {
                _index = index;
                _move.Run(this, new(currentPosition.x * _directSize.x * 0.5f, currentPosition.y * _directSize.y * 0.5f, 0f));
            }
            return currentPosition + _directSize;
        }

        private IEnumerator Work_Cn(IEnumerator delay)
        {
            yield return _waitSwitch.Show();
            yield return delay;
            yield return _waitSwitch.Hide();

            _move.Skip();
            _coroutine = null;
            _endWork.Invoke();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _endWork.Invoke();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _windowImage);
            this.SetChildren(ref _textTMP);

            if (_waitSwitch.CanvasGroup == null)
                _waitSwitch.OnValidate(GetComponentInChildren<CanvasGroup>());
        }
#endif
    }
}
