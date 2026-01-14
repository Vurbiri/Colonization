using UnityEditor;

namespace VurbiriEditor.International
{
	using static CONST;

	internal class LanguageFilesProvider
	{
		[SettingsProvider]
		public static SettingsProvider CreateProjectSettingsProvider()
		{
			var provider = new SettingsProvider(PROJECT_FILES_MENU, SettingsScope.Project)
			{
				label = PROJECT_FILES_LABEL,
				activateHandler = LanguageFilesEditor.Load,
				deactivateHandler = LanguageFilesEditor.Unload
            };

			return provider;
		}
	}
}
