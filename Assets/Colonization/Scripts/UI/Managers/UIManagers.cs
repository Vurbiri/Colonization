using UnityEngine;
using Vurbiri.Colonization.EntryPoint;

namespace Vurbiri.Colonization.UI
{
	public class UIManagers : MonoBehaviour
	{
        [SerializeField] private GameManager _game;
        [SerializeField] private ContextMenusManager _contextMenus;
        [SerializeField] private WindowsManager _windows;

        public void Init(GameContent content)
        {
            _game.Init(content.gameLoop, content.cameraController, this);
            _contextMenus.Init(new(content.players, content.worldHint, content.cameraTransform));
            _windows.Init(content);
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
