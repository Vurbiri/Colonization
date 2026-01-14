using UnityEditor;

namespace VurbiriEditor.International
{
	using static CONST;

	public class LanguageStringsProvider
	{
		[SettingsProvider]
		public static SettingsProvider CreateProjectSettingsProvider()
		{
			LanguageStringsScriptable strings = LanguageStringsScriptable.GetOrCreateSelf();

			var provider = new SettingsProvider(PROJECT_STRING_MENU, SettingsScope.Project)
			{
				label = PROJECT_STRING_LABEL,
				activateHandler = (searchContext, rootElement)
								=> rootElement.Add(LanguageStringsEditor.CreateCachedEditorAndBind(strings)),
				deactivateHandler = strings.Dispose
			};

			return provider;
		}
	}
}
