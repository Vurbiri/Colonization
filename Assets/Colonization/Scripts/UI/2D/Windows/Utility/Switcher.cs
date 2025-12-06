using System;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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

        [Impl(256)] public void Init(MonoBehaviour parent)
        {
            _parent = parent;
            _canvasSwitcher.Set(_isOpen = false);

            Transition.OnExit.Add(OnSceneExit);
        }
        [Impl(256)] public Switcher Setup(int id, Action<int> onOpenWindow, Action onCloseWindow)
        {
            _id = id;
            onOpen.Add(onOpenWindow);
            onClose.Add(onCloseWindow);
            return this;
        }
                
        public void Switch()
        {
            StopCoroutine();

            if (_isOpen = !_isOpen)
                OpenInternal();
            else
                CloseInternal();
        }

        public void Open()
        {
            if (!_isOpen)
            {
                _isOpen = true;
                StopCoroutine();
                OpenInternal();
            }
        }

        public void Close()
        {
            if (_isOpen)
            {
                _isOpen = false;
                StopCoroutine();
                CloseInternal();
            }
        }

        [Impl(256)] public void TryClose(int id)
        {
            if (id != _id) Close();
        }

        [Impl(256)] private void OpenInternal()
        {
            _parent.StartCoroutine(_canvasSwitcher.Show());
            onOpen.Invoke(_id);
        }

        [Impl(256)] private void CloseInternal()
        {
            _parent.StartCoroutine(_canvasSwitcher.Hide());
            onClose.Invoke();
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
        public void OnValidate(MonoBehaviour parent)
        {
            _canvasSwitcher.OnValidate(parent, 6);
        }
#endif
    }
}
