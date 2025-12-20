using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    public partial class Banner : MonoBehaviour
	{
        private static readonly List<Banner> s_banners = new();
        private static readonly Stack<Banner> s_pool = new();

        private static Settings s_settings;
        private static Vector2 s_currentPosition;

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

        public static IdArray<MessageTypeId, Color> Colors => s_settings.colors;

        internal static void Init(Settings settings) => s_settings = settings;

        public static void Open(string text, Color color, IEnumerator delay, bool oneScene = false)
        {
            Banner banner = s_pool.Count > 0 ? s_pool.Pop() : s_settings.Create();
            banner.Setup(s_banners.Count, text, color, delay, oneScene);
            s_banners.Add(banner);
        }
        public static void Open(string text, Id<MessageTypeId> typeId, IEnumerator delay, bool oneScene = false) => Open(text, s_settings.colors[typeId], delay, oneScene);
        public static WaitRealtime Open(string text, Color color, float time, bool oneScene = false)
        {
            WaitRealtime waitTime = new(time);
            Open(text, color, waitTime, oneScene);
            return waitTime;
        }
        public static WaitRealtime Open(string text, Id<MessageTypeId> typeId, float time, bool oneScene = false) => Open(text, s_settings.colors[typeId], time, oneScene);

        public static void Clear()
        {
            for(int i = s_banners.Count - 1; i >= 0; i--)
                s_banners[i].Stop();

            s_currentPosition = Vector3.zero;
        }

        private Banner Init(float speed)
        {
            _windowTransform = _windowImage.rectTransform;
            _textTransform = _textTMP.rectTransform;

            _textTMP.enableWordWrapping = true;
            _textTMP.overflowMode = TextOverflowModes.Overflow;

            _waitSwitch.Disable();
            _move = new(_windowTransform, speed);

            return this;
        }

        private void Setup(int index, string text, Color color, IEnumerator delay, bool oneScene)
        {
            _index = index;

            _outline.effectColor = color;

            _textTransform.sizeDelta = s_settings.maxSize;
            _textTMP.text = text;
            _textTMP.color = color;
            _textTMP.ForceMeshUpdate();

            Vector2 size = _textTMP.textBounds.size;

            _textTransform.sizeDelta = size;
            _windowTransform.sizeDelta = size += s_settings.padding;

            _directSize = new(size.x * s_settings.direction.x, size.y * s_settings.direction.y);
            _windowTransform.localPosition = new(s_currentPosition.x + _directSize.x * 0.5f, s_currentPosition.y + _directSize.y * 0.5f, 0f);

            _coroutine = StartCoroutine(Work_Cn(delay));
            if (oneScene) SceneManager.sceneUnloaded += OnSceneUnloaded;

            s_currentPosition += s_settings.space + _directSize;
        }

        private void SetPosition(int index)
        {
            if (_index != index)
            {
                _index = index;
                _move.Run(this, new(s_currentPosition.x + _directSize.x * 0.5f, s_currentPosition.y + _directSize.y * 0.5f, 0f));
            }

            s_currentPosition += s_settings.space + _directSize;
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

        #region Nested Settings
#pragma warning disable 649
        //******************************************************
        [System.Serializable]
        internal class Settings
        {
            [SerializeField] private Banner _prefab;
            [SerializeField] private Transform _container;
            [Space]
            [SerializeField] private float _moveSpeed;
            [Space]
            public Vector2 maxSize;
            public Vector2 padding;
            [Space]
            public IdArray<MessageTypeId, Color> colors;

            [NonSerialized] public Vector2 direction;
            [NonSerialized] public Vector2 space;

            public Settings Init(Vector2 direction, float space, int startCount)
            {
                this.direction = direction;
                this.space = direction * space;

                for (int i = 0; i < startCount; i++)
                    s_pool.Push(Create());

                return this;
            }

            [Impl(256)] public Banner Create() => Instantiate(_prefab, _container, false).Init(_moveSpeed);


#if UNITY_EDITOR
            internal void OnValidate(BannerInitialize init)
            { 
                EUtility.SetPrefab(ref _prefab);
                init.SetComponent(ref _container);
            }
#endif
        }
#pragma warning restore 649
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _windowImage);
            this.SetChildren(ref _textTMP);
            this.SetChildren(ref _outline);

            _waitSwitch.OnValidate(this, 7);
        }
#endif
    }
}
