using UnityEditor;

namespace VurbiriEditor.International
{
	using static CONST;

	public class LanguageStringsProvider
	{
		[SettingsProvider]
		public static SettingsProvider CreateProjectSettingsProvider()
		{
			var provider = new SettingsProvider(PROJECT_STRING_MENU, SettingsScope.Project)
			{
				label = PROJECT_STRING_LABEL,
				activateHandler = LanguageStringsEditor.Load,
				deactivateHandler = LanguageStringsEditor.Unload
			};

			return provider;
		}
	}
}
