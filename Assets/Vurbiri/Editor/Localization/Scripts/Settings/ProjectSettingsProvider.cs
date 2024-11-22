//Assets\Vurbiri\Editor\Localization\Scripts\Settings\ProjectSettingsProvider.cs
using UnityEditor;

namespace Vurbiri.Localization.Editors
{
    using static CONST;

    internal class ProjectSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProjectSettingsProvider()
        {
            var provider = new SettingsProvider(PROJECT_MENU, SettingsScope.Project)
            {
                label = PROJECT_LABEL,
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(ProjectSettingsEditor.CreateCachedEditorAndBind(ProjectSettingsScriptable.GetOrCreateSelf()))
            };

            return provider;
        }
    }
}
