//Assets\Vurbiri.International\Editor\Scripts\CONST.cs
using System.Text;

namespace Vurbiri.International.Editor
{
    internal class CONST
    {
        public const string FOLDER = "/Vurbiri.International/";

        public const string JSON_EXP = ".json";
        public const string ASSET_EXP = ".asset";

        public const string IN_RESOURCE_FOLDER = FOLDER + "Editor/Resources/";
        public const string OUT_RESOURCE_FOLDER = "/Localization/Resources/";

        public const string ASSET_FOLDER = "Assets" + IN_RESOURCE_FOLDER;

        public const string FILE_LANG = CONST_L.FILE_LANG;
        public const string FILE_LANG_PATH = OUT_RESOURCE_FOLDER + FILE_LANG + JSON_EXP;

        public const string LANG_LIST = "LanguageList";

        public const string LANG_FILES_NAME = "LanguageFiles";
        public const string LANG_FILES_PATH = ASSET_FOLDER + LANG_FILES_NAME + ASSET_EXP;

        public const string LANG_TYPES_NAME = "LanguageTypes";
        public const string LANG_TYPES_PATH = ASSET_FOLDER + LANG_TYPES_NAME + ASSET_EXP;

        public const string LANG_STRING_NAME = "LanguageStrings";
        public const string LANG_STRING_PATH = ASSET_FOLDER + LANG_STRING_NAME + ASSET_EXP;
        

        public const string PROJECT_LABEL = "Localization";
        public const string PROJECT_MENU = "Project/Localization";

        public const string PROJECT_TYPES_LABEL = "Languages";
        public const string PROJECT_TYPES_MENU = PROJECT_MENU + "/Languages";

        public const string PROJECT_STRING_LABEL = "Strings";
        public const string PROJECT_STRING_MENU = PROJECT_MENU + "/Strings";

        public static readonly Encoding utf8WithoutBom = new UTF8Encoding(false);
    }
}
