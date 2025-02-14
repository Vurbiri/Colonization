//Assets\Colonization\Editor\Actors\Utility\Localization.cs
using Vurbiri.Localization;

namespace VurbiriEditor.Colonization
{
    public static class Localization
	{
        private static readonly Language _instance;

        static Localization()
        {
            _instance = new(new bool[] { false, false, true });
            _instance.SwitchLanguage(0);
        }

        public static string GetText(Files file, string key) => _instance?.GetText(file, key);
        public static string GetTextFormat(Files file, string key, params object[] args) => _instance?.GetTextFormat(file, key, args);
    }
}
