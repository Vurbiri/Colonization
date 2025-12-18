using System;
using System.Collections;
using UnityEngine;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
	[System.Serializable]
	public class Switcher
	{
        [SerializeField] private CanvasGroupSwitcher _canvasSwitcher;

        private MonoBehaviour _parent;
        private bool _isOpen, _isForceClose;

        public readonly ComboAction<Switcher> onOpen = new();
        public readonly ComboAction<Switcher> onClose = new();

        public bool IsOpen { [Impl(256)] get => _isOpen; }

        public Switcher() { }

        [Impl(256)] public void Init(MonoBehaviour parent, bool isForceClose = true)
        {
            _parent = parent;
            _isForceClose = isForceClose;
            _canvasSwitcher.Set(_isOpen = false);

            Vurbiri.EntryPoint.Transition.OnExit.Add(OnSceneExit);
        }
        [Impl(256)] public Switcher Setup(Action<Switcher> onOpenWindow, Action<Switcher> onCloseWindow)
        {
            onOpen.Add(onOpenWindow);
            onClose.Add(onCloseWindow);
            return this;
        }

        [Impl(256)] public void Switch()
        {
            if (_isOpen)
                CloseInternal();
            else
                OpenInternal();
        }

        [Impl(256)] public void Open()
        {
            if (!_isOpen)
                OpenInternal();
        }
        [Impl(256)] public void Close()
        {
            if (_isOpen)
                CloseInternal();
        }

        public void SilentOpen()
        {
            if (!_isOpen)
            {
                StopCoroutine();
                _parent.StartCoroutine(Show());
            }
        }
        public void SilentClose()
        {
            if (_isOpen)
            {
                StopCoroutine();
                _parent.StartCoroutine(Hide());
            }
        }

        [Impl(256)] public void ForceClose()
        {
            if (_isForceClose & _isOpen) 
                CloseInternal();
        }

        [Impl(256)] public IEnumerator Show()
        {
            _isOpen = true;
            return _canvasSwitcher.Show();
        }
        [Impl(256)] public IEnumerator Hide()
        {
            _isOpen = false;
            return _canvasSwitcher.Hide();
        }

        [Impl(256)] private void OpenInternal()
        {
            _isOpen = true;
            StopCoroutine();
            _parent.StartCoroutine(_canvasSwitcher.Show());
            onOpen.Invoke(this);
        }

        [Impl(256)] private void CloseInternal()
        {
            _isOpen = false;
            StopCoroutine();
            _parent.StartCoroutine(_canvasSwitcher.Hide());
            onClose.Invoke(this);
        }

        [Impl(256)] private void StopCoroutine()
        {
            if (_canvasSwitcher.IsRunning)
                _parent.StopCoroutine(_canvasSwitcher);
        }

        private void OnSceneExit()
        {
            onOpen.Clear();
            onClose.Clear();
        }

#if UNITY_EDITOR

        public Switcher(MonoBehaviour parent)
        {
            _canvasSwitcher = new();
            _canvasSwitcher.OnValidate(parent, 6);
        }

        public void OnValidate(MonoBehaviour parent)
        {
            _canvasSwitcher.OnValidate(parent, 6);
        }
#endif
    }
}
