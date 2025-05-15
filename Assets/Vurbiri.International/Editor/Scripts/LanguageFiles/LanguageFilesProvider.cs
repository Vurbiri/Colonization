//Assets\Vurbiri.International\Editor\Scripts\LanguageFiles\LanguageFilesProvider.cs
using UnityEditor;

namespace Vurbiri.International.Editor
{
    using static CONST;

    internal class LanguageFilesProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProjectSettingsProvider()
        {
            LanguageFilesScriptable files = LanguageFilesScriptable.GetOrCreateSelf();
            
            var provider = new SettingsProvider(PROJECT_MENU, SettingsScope.Project)
            {
                label = PROJECT_LABEL,
                activateHandler = (searchContext, rootElement)
                                => rootElement.Add(LanguageFilesEditor.CreateCachedEditorAndBind(files)),
                 deactivateHandler = files.Dispose
            };

            return provider;
        }
    }
}
