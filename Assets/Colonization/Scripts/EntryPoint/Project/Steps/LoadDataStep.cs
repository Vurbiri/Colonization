//Assets\Colonization\Scripts\EntryPoint\Project\Steps\LoadDataStep.cs
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class LoadDataStep : ALoadingStep
    {
        private readonly DIContainer _diContainer;
        private readonly PlayerVisualSetScriptable _playerVisualSetScriptable;

        public LoadDataStep(DIContainer diContainer, PlayerVisualSetScriptable playerVisual) : base("LoadDataStep")
        {
            _diContainer = diContainer;
            _playerVisualSetScriptable = playerVisual;
        }

        public override bool MoveNext()
        {
            var projectStorage = _diContainer.Get<ProjectStorage>();

            _playerVisualSetScriptable.Init(projectStorage, _diContainer);
            _diContainer.AddInstance(new GameSettings(projectStorage));

            projectStorage.Save();

            return false;
        }
    }
}
