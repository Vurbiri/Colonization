using System;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class WindowItem
    {
        [SerializeField] private ASwitchableWindow _window;
        [SerializeField] private AVButton _button;

        [Impl(256)]
        public void Init(Action<Switcher> onOpenWindow, Action<Switcher> onCloseWindow)
        {
            var switcher = _window.Init().Setup(onOpenWindow, onCloseWindow);
            _button.AddListener(switcher.Switch);
        }

        [Impl(256)]
        public void Init(Action<Switcher> onOpenWindow, Action<Switcher> onCloseWindow, ref AVButton button)
        {
            var switcher = _window.Init().Setup(onOpenWindow, onCloseWindow);
            
            button = _button;
            button.AddListener(switcher.Switch);
        }

#if UNITY_EDITOR
        public void OnValidate(string window, string button)
        {
            EUtility.SetObject(ref _window, window);
            EUtility.SetObject(ref _button, button);
        }
#endif
    }
}
