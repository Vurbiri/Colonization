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
                keywords = new[] { FOLDER, FILE_LANG },
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(ProjectSettingsEditor.GetVisualElement(ProjectSettingsScriptable.GetSerializedSelf()))
            };

            return provider;
        }
    }
}
