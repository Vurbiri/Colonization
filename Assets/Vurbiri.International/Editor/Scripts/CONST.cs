using System.Text;

namespace Vurbiri.International.Editor
{
    internal class CONST
    {
        public const string ROOT_FOLDER = "Assets";
        public const string ASM_FOLDER = ROOT_FOLDER + "/Vurbiri.International/";

        public const string JSON_EXP = ".json";
        public const string ASSET_EXP = ".asset";

        public const string IN_RESOURCE_FOLDER = ASM_FOLDER + "Editor/Resources/";
        public const string OUT_RESOURCE_FOLDER = "Assets/Localization/Resources/";

        public const string FILE_LANG = CONST_L.FILE_LANG;
        public const string FILE_LANG_PATH = OUT_RESOURCE_FOLDER + FILE_LANG + JSON_EXP;

        public const string FILE_FILES = CONST_L.FILE_FILES;
        public const string FILE_FILES_PATH = OUT_RESOURCE_FOLDER + FILE_FILES + JSON_EXP;

        public const string LANG_LIST = "LanguageList";

        public const string LANG_TYPES_NAME = "LanguageTypes";
        public const string LANG_TYPES_PATH = IN_RESOURCE_FOLDER + LANG_TYPES_NAME + ASSET_EXP;

        public const string LANG_FILES_NAME = "LanguageFiles";
        public const string LANG_FILES_PATH = IN_RESOURCE_FOLDER + LANG_FILES_NAME + ASSET_EXP;

        public const string LANG_STRING_NAME = "LanguageStrings";
        public const string LANG_STRING_PATH = IN_RESOURCE_FOLDER + LANG_STRING_NAME + ASSET_EXP;
        

        public const string PROJECT_TYPES_LABEL = "Localization";
        public const string PROJECT_TYPES_MENU = "Project" + _SLASH  + PROJECT_TYPES_LABEL;

        public const string PROJECT_FILES_LABEL = "Files";
        public const string PROJECT_FILES_MENU = PROJECT_TYPES_MENU + _SLASH + PROJECT_FILES_LABEL;

        public const string PROJECT_STRING_LABEL = "Strings";
        public const string PROJECT_STRING_MENU = PROJECT_TYPES_MENU + _SLASH + PROJECT_STRING_LABEL;

        public static readonly Encoding utf8WithoutBom = new UTF8Encoding(false);

        private const string _SLASH = "/";
    }
}
