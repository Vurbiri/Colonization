//Assets\Vurbiri\Editor\Localization\Scripts\Settings\ProjectSettingsProvider.cs
using UnityEditor;

namespace Vurbiri.Localization.Editor
{
    using static CONST;

    internal class ProjectSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProjectSettingsProvider()
        {
            ProjectSettingsScriptable settings = ProjectSettingsScriptable.GetOrCreateSelf();
            
            var provider = new SettingsProvider(PROJECT_MENU, SettingsScope.Project)
            {
                label = PROJECT_LABEL,
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(ProjectSettingsEditor.CreateCachedEditorAndBind(settings)),
                 deactivateHandler = settings.Dispose
            };

            return provider;
        }
    }
}
