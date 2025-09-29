using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class LoadSettingsStep : ALocalizationLoadingStep
    {
        private readonly ProjectContent _content;
        private readonly PlayerVisualSetScriptable _playerVisualSetScriptable;

        public LoadSettingsStep(ProjectContent content, PlayerVisualSetScriptable playerVisual) : base("LoadingSettingsStep")
        {
            _content = content;
            _playerVisualSetScriptable = playerVisual;
        }

        public override IEnumerator GetEnumerator()
        {
            _playerVisualSetScriptable.Init(_content);

            _content.gameSettings = _content.projectStorage.LoadGameSettings();

            _content.projectStorage.Save();

            yield break;
        }
    }
}
