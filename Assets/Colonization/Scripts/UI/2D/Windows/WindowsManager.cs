using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
	{
        [SerializeField] private PerksWindow _perksWindow;
        [SerializeField] private HintButton _perksButton;

        public void Init(GameEvents game, Human player, CanvasHint hint)
        {
            _perksWindow.Init(player, hint);
            _perksButton.Init(hint, _perksWindow.Switch/*, false*/);

            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.Play, OnPlay);
        }
        private void OnPlay(TurnQueue turnQueue, int hexId)
        {
            if (!turnQueue.IsPlayer) return;

            _perksButton.Interactable = true;
        }
        private void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            _perksButton.Interactable = false;
            _perksWindow.Close();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref _perksWindow);
            EUtility.SetObject(ref _perksButton, "PerksButton");
        }
#endif
    }
}
