using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class UIManagers : MonoBehaviour
	{
        [SerializeField] private GameManager _game;
        [SerializeField] private ContextMenusManager _contextMenus;
        [SerializeField] private WindowsManager _windows;

        public void Init()
        {
            _game.Init(this);
            _contextMenus.Init();
            _windows.Init();
        }

        public void OnDestroy()
        {
            _contextMenus.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _game.OnValidate();
            _contextMenus.OnValidate();
            _windows.OnValidate();
        }
#endif
	}
}
