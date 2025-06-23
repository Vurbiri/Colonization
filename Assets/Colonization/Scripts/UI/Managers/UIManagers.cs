using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class UIManagers : MonoBehaviour
	{
        [SerializeField] private GameManager _game;
        [SerializeField] private WindowsManager _windows;

        public void Init(GameLoop game, CameraController camera, Human player, CanvasHint hint)
        {
            _game.Init(game, camera, this);
            _windows.Init(game, player, hint);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _game.OnValidate();
            _windows.OnValidate();
        }
#endif
	}
}
