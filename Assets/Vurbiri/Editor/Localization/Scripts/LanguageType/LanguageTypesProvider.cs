//Assets\Vurbiri\Editor\Localization\Scripts\LanguageType\LanguageTypesProvider.cs
using UnityEditor;

namespace Vurbiri.Localization.Editor
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
                                => rootElement.Add(LanguageTypesEditor.CreateCachedEditorAndBind(LanguageTypesScriptable.GetOrCreateSelf()))
            };

            return provider;
        }
    }
}
