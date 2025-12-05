using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	[System.Serializable]
	public class Switcher
	{
        [SerializeField] private CanvasGroupSwitcher _canvasSwitcher;

        private MonoBehaviour _parent;
        private bool _isOpen;
        private int _id;

        public readonly ComboAction<int> onOpen = new();
        public readonly VAction onClose = new();

        public bool IsOpen => _isOpen;

        public void Init(MonoBehaviour parent)
        {
            _parent = parent;
            _canvasSwitcher.Set(_isOpen = false);

            Transition.OnExit.Add(OnSceneExit);
        }
        public Switcher Setup(int id, Action<int> onOpenWindow, Action onCloseWindow)
        {
            _id = id;
            onOpen.Add(onOpenWindow);
            onClose.Add(onCloseWindow);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Switch()
        {
            StopCoroutine();

            if (_isOpen = !_isOpen)
                InternalOpen();
            else
                InternalClose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open()
        {
            if (!_isOpen)
            {
                _isOpen = true;
                StopCoroutine();
                InternalOpen();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Close()
        {
            if (_isOpen)
            {
                _isOpen = false;
                StopCoroutine();
                InternalClose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryClose(int id)
        {
            if (_isOpen & id != _id)
            {
                _isOpen = false;
                StopCoroutine();
                InternalClose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InternalOpen()
        {
            _parent.StartCoroutine(_canvasSwitcher.Show());
            onOpen.Invoke(_id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InternalClose()
        {
            _parent.StartCoroutine(_canvasSwitcher.Hide());
            onClose.Invoke();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StopCoroutine()
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
        public void OnValidate(MonoBehaviour parent)
        {
            _canvasSwitcher.OnValidate(parent, 6);
        }
#endif
    }
}
