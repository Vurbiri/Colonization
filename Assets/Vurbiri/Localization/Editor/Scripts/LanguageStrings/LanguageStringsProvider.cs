using UnityEditor;

namespace Vurbiri.Localization.Editors
{
    using static CONST;

    public class LanguageStringsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProjectSettingsProvider()
        {
            LanguageStringsScriptable strings = LanguageStringsScriptable.GetOrCreateSelf();

            var provider = new SettingsProvider(STR_PROJECT_MENU, SettingsScope.Project)
            {
                label = STR_PROJECT_LABEL,
                keywords = new[] { FOLDER, FILE_LANG },
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(LanguageStringsEditor.GetVisualElement(new(strings))),
                deactivateHandler = strings.Uninitialized
            };

            return provider;
        }
    }
}
