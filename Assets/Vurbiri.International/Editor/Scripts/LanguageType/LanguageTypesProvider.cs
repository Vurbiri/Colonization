//Assets\Vurbiri.International\Editor\Scripts\LanguageType\LanguageTypesProvider.cs
using UnityEditor;

namespace Vurbiri.International.Editor
{
    using static CONST;

    internal class LanguageTypesProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProjectSettingsProvider()
        {
            var provider = new SettingsProvider(PROJECT_TYPES_MENU, SettingsScope.Project)
            {
                label = PROJECT_TYPES_LABEL,
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(LanguageTypesEditor.CreateCachedEditorAndBind(LanguageTypesScriptable.GetOrCreateSelf()))
            };

            return provider;
        }
    }
}
