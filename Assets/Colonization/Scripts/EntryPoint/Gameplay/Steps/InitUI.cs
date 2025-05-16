//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\InitUI.cs
using System.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class InitUI : ALocalizationLoadingStep
    {
        private readonly PlayerPanels _playerPanelsUI;
        private readonly InputController _inputController;

        public InitUI(PlayerPanels playerPanelsUI, InputController inputController) : base("InitUIStep")
        {
            _playerPanelsUI = playerPanelsUI;
            _inputController = inputController;
        }

        public override IEnumerator GetEnumerator()
        {
            yield return null;
            _playerPanelsUI.Init(_inputController);
            yield return null;
        }
    }
}
