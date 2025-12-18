using System;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class MainMenuManager : MonoBehaviour
	{
        private const int MENU_ITEM_COUNT = 1;
        
        [SerializeField] private VButton _continue;
        [SerializeField] private WindowItem[] _items;

		private readonly Switcher[] _windows = new Switcher[MENU_ITEM_COUNT];

        private void Start()
        {
			var game = ProjectContainer.GameSettings;

			if (_continue.interactable = game.IsLoad)
                _continue.AddListener(Vurbiri.EntryPoint.Transition.Exit);


            
        }

        

        [Serializable]
		private class WindowItem
		{
            [SerializeField] private ASwitchableWindow _window;
            [SerializeField] private VButton _button;

            [Impl(256)]
            public Switcher Init(int id, Action<int> onOpenWindow)
            {
                var switcher = _window.Init();//.Setup(id, onOpenWindow);
                _button.AddListener(switcher.Switch);
                return switcher;
            }
        }

        private class Menu
        {
            private readonly Switcher[] _windows = new Switcher[MENU_ITEM_COUNT];
            private Switcher _current;

            public Menu(WindowItem[] items)
            {
                for (int i = 0; i < MENU_ITEM_COUNT; ++i)
                    _windows[i] = items[i].Init(i, OnOpenWindow);
            }

            private void OnOpenWindow(int id)
            {
                
            }

            private void OnCloseWindow(int id)
            {
               
            }
        }

    }
}
