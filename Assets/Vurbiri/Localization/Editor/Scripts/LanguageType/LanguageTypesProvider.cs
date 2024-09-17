using UnityEditor;

namespace Vurbiri.Localization.Editors
{
    using static CONST;

    internal class LanguageTypesProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProjectSettingsProvider()
        {
            var provider = new SettingsProvider(LANG_PROJECT_MENU, SettingsScope.Project)
            {
                label = LANG_PROJECT_LABEL,
                keywords = new[] { FOLDER, FILE_LANG },
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(LanguageTypesEditor.GetVisualElement(LanguageTypesScriptable.GetSerializedSelf()))
            };

            return provider;
        }
    }
}
