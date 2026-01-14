using UnityEditor;

namespace VurbiriEditor.International
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
				activateHandler = LanguageTypesEditor.Load,
				deactivateHandler = LanguageTypesEditor.Unload
			};

			return provider;
		}
	}
}
