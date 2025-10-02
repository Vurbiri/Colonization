using System.Text;

namespace VurbiriEditor.Colonization
{
    internal static class CONST_EDITOR
    {
        public const string MENU_PATH = "Colonization/";
        public const string MENU_CH_PATH = MENU_PATH + "Characteristics/";
        public const string MENU_AI_PATH = MENU_PATH + "AI/";
        public const string MENU_GS_PATH = MENU_PATH + "Game States/";
        public const string MENU_UI_PATH = MENU_PATH + "UI/";
        public const string MENU_AC_PATH = MENU_PATH + "Actors/";

        public const string MENU_PERKS_PATH = MENU_CH_PATH + "Perks/";
        public const string MENU_SATAN_PATH = MENU_CH_PATH + "Satan/";
        public const int SPACE_WND = 8;

        public const string SETTINGS_PATH = "Assets/Colonization/Settings/";
        public const string SETTINGS_UI_PATH = SETTINGS_PATH + "UI/";

        public const int MENU_AC_ORDER = 11;
        public const int MENU_AI_ORDER = 12;

        public const int MENU_PERKS_ORDER = 13;
        public const int MENU_SATAN_ORDER = 14;
        public const int MENU_CH_ORDER = 15;

        public const int MENU_GS_ORDER = 30;
        

        public static readonly Encoding utf8WithoutBom = new UTF8Encoding(false);
    }
}
