using UnityEngine;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class UIManagers : MonoBehaviour
	{
        [SerializeField] private GameManager _game;
        [SerializeField] private ContextMenusManager _contextMenus;
        [SerializeField] private WindowsManager _windows;

        public void Init(GameplayContent init, CanvasHint hint, ContextMenuSettings settings)
        {
            _game.Init(init.gameLoop, init.cameraController, this);
            _contextMenus.Init(settings);
            _windows.Init(init, hint);
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
