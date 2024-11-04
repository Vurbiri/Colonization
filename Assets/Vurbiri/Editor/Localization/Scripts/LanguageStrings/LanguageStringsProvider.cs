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
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(LanguageStringsEditor.CreateCachedEditorAndBind(strings)),
                deactivateHandler = strings.UnInit
            };

            return provider;
        }
    }
}
