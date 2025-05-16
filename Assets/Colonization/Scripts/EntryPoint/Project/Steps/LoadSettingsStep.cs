//Assets\Colonization\Scripts\EntryPoint\Project\Steps\LoadSettingsStep.cs
using System.Collections;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class LoadSettingsStep : ALocalizationLoadingStep
    {
        private readonly DIContainer _diContainer;
        private readonly PlayerVisualSetScriptable _playerVisualSetScriptable;

        public LoadSettingsStep(DIContainer diContainer, PlayerVisualSetScriptable playerVisual) : base("LoadingSettingsStep")
        {
            _diContainer = diContainer;
            _playerVisualSetScriptable = playerVisual;
        }

        public override IEnumerator GetEnumerator()
        {
            var projectStorage = _diContainer.Get<ProjectStorage>();

            _playerVisualSetScriptable.Init(projectStorage, _diContainer);
            _diContainer.AddInstance(new GameState(projectStorage));

            projectStorage.Save();

            yield break;
        }
    }
}
