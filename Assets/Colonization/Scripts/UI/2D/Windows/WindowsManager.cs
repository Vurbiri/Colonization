using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
	{
        [SerializeField] private PerksWindow _perksWindow;
        [SerializeField] private HintButton _perksButton;
        [Space]
        [SerializeField] private ExchangeWindow _exchangeWindow;
        [SerializeField] private HintButton _exchangeButton;

        public void Init(GameEvents game, Human player, CanvasHint hint)
        {
            Debug.Log("WindowsManager - убрать комментарии - /*, false*/");
            
            _perksWindow.Init(player, hint, _exchangeWindow.Close);
            _perksButton.Init(hint, _perksWindow.Switch/*, false*/);

            _exchangeWindow.Init(player, hint, _perksWindow.Close);
            _exchangeButton.Init(hint, _exchangeWindow.Switch/*, false*/);

            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.Play, OnPlay);
        }
        private void OnPlay(TurnQueue turnQueue, int hexId)
        {
            if (!turnQueue.IsPlayer) return;

            _perksButton.Interactable = true;
            _exchangeButton.Interactable = true;
        }
        private void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            _perksButton.Interactable = false;
            _exchangeButton.Interactable = false;
            _perksWindow.Close();
            _exchangeWindow.Close();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref _perksWindow);
            EUtility.SetObject(ref _perksButton, "PerksButton");

            EUtility.SetObject(ref _exchangeWindow);
            EUtility.SetObject(ref _exchangeButton, "ExchangeButton");
        }
#endif
    }
}
