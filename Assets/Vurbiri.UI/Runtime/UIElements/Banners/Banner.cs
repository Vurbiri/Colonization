using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vurbiri.Collections;

namespace Vurbiri.UI
{
    public partial class Banner : MonoBehaviour
	{
        private static readonly List<Banner> s_banners = new();
        private static readonly Stack<Banner> s_pool = new();

        private static Vector2 s_maxSize, s_padding, s_direction, s_space;
        private static Vector2 s_currentPosition;
        private static float s_moveSpeed;

        private static Banner s_prefab;
        private static Transform s_container;
        private static IdArray<MessageTypeId, Color> s_colors;

        [SerializeField] private Image _windowImage;
        [SerializeField] private TextMeshProUGUI _textTMP;
        [SerializeField] private Outline _outline;
        [Space]
        [SerializeField] private CanvasGroupSwitcher _waitSwitch;

        private MoveUsingLerp _move;
        private RectTransform _windowTransform, _textTransform;
        private Vector2 _directSize;
        private Coroutine _coroutine;
        private int _index;

        public static IdArray<MessageTypeId, Color> Colors => s_colors;

        internal static void Init(BannerInitialize init)
        {
            s_maxSize = init.maxSize; s_padding = init.padding; 
            s_direction = init.direction; s_space = s_direction * init.space;
            s_moveSpeed = init.moveSpeed;
            s_prefab = init.prefab; s_container = init.container; s_colors = init.colors;

            for (int i = 0; i < init.startCount; i++)
                s_pool.Push(Instantiate(s_prefab, s_container, false).Init());
        }
        public static void Open(string text, Color color, IEnumerator delay, bool oneScene = false)
        {
            Banner banner;
            if (s_pool.Count > 0)
                banner = s_pool.Pop();
            else
                banner = Instantiate(s_prefab, s_container, false).Init();

            banner.Setup(s_banners.Count, text, color, delay, oneScene);
            s_banners.Add(banner);
        }
        public static void Open(string text, Id<MessageTypeId> typeId, IEnumerator delay, bool oneScene = false) => Open(text, s_colors[typeId], delay, oneScene);
        public static WaitRealtime Open(string text, Color color, float time, bool oneScene = false)
        {
            WaitRealtime waitTime = new(time);
            Open(text, color, waitTime, oneScene);
            return waitTime;
        }
        public static WaitRealtime Open(string text, Id<MessageTypeId> typeId, float time, bool oneScene = false) => Open(text, s_colors[typeId], time, oneScene);

        public static void Clear()
        {
            for(int i = s_banners.Count - 1; i >= 0; i--)
                s_banners[i].Stop();

            s_currentPosition = Vector3.zero;
        }

        private Banner Init()
        {
            _windowTransform = _windowImage.rectTransform;
            _textTransform = _textTMP.rectTransform;

            _textTMP.enableWordWrapping = true;
            _textTMP.overflowMode = TextOverflowModes.Overflow;

            _waitSwitch.Disable();
            _move = new(_windowTransform, s_moveSpeed);

            return this;
        }

        private void Setup(int index, string text, Color color, IEnumerator delay, bool oneScene)
        {
            _index = index;

            _outline.effectColor = color;

            _textTransform.sizeDelta = s_maxSize;
            _textTMP.text = text;
            _textTMP.color = color;
            _textTMP.ForceMeshUpdate();

            Vector2 size = _textTMP.textBounds.size;

            _textTransform.sizeDelta = size;
            _windowTransform.sizeDelta = size += s_padding;

            _directSize = new(size.x * s_direction.x, size.y * s_direction.y);
            _windowTransform.localPosition = new(s_currentPosition.x + _directSize.x * 0.5f, s_currentPosition.y + _directSize.y * 0.5f, 0f);

            _coroutine = StartCoroutine(Work_Cn(delay));
            if (oneScene) SceneManager.sceneUnloaded += OnSceneUnloaded;

            s_currentPosition += s_space + _directSize;
        }

        private void SetPosition(int index)
        {
            if (_index != index)
            {
                _index = index;
                _move.Run(this, new(s_currentPosition.x + _directSize.x * 0.5f, s_currentPosition.y + _directSize.y * 0.5f, 0f));
            }

            s_currentPosition += s_space + _directSize;
        }

        private IEnumerator Work_Cn(IEnumerator delay)
        {
            yield return _waitSwitch.Show();
            yield return delay;
            while (_move.IsWait) yield return null;
            yield return _waitSwitch.Hide();

            _coroutine = null;
            EndWork(this);
        }

        private static void EndWork(Banner banner)
        {
            banner.Stop(); 

            s_currentPosition = Vector3.zero;
            for (int index = 0; index < s_banners.Count; index++)
                s_banners[index].SetPosition(index);
        }

        private void Stop()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _move.Stop(); _waitSwitch.Disable();

            s_banners.RemoveAt(_index);
            s_pool.Push(this);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            EndWork(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _windowImage);
            this.SetChildren(ref _textTMP);
            this.SetChildren(ref _outline);

            _waitSwitch.OnValidate(this);
        }
#endif
    }
}
