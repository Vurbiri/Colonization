using System.Text;

namespace VurbiriEditor.Colonization
{
    internal static class CONST_EDITOR
    {
        public const string MENU_PATH = "Colonization/";
        public const string MENU_CH_PATH = MENU_PATH + "Characteristics/";
        public const string MENU_GS_PATH = MENU_PATH + "Game States/";
        public const string MENU_UI_PATH = MENU_PATH + "UI/";
        public const string MENU_PERKS_PATH = MENU_CH_PATH + "Perks/";
        public const string MENU_BUFFS_PATH = MENU_CH_PATH + "Buffs/";
        public const int SPACE_WND = 8;

        public static readonly Encoding utf8WithoutBom = new UTF8Encoding(false);
    }
}
