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
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(LanguageTypesEditor.BindAndGetVisualElement(LanguageTypesScriptable.GetOrCreateSelf()))
            };

            return provider;
        }
    }
}
