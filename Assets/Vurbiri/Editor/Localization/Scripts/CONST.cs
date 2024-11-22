//Assets\Vurbiri\Editor\Localization\Scripts\CONST.cs
namespace Vurbiri.Localization.Editors
{
    using GLOBAL_CONST = Vurbiri.Localization.CONST_L;

    public class CONST
    {
        public const string FOLDER = GLOBAL_CONST.FOLDER;
        public const string FILE_LANG = GLOBAL_CONST.FILE_LANG;

        public const string JSON_EXP = ".json";

        public const string ASSET_EXP = GLOBAL_CONST.ASSET_EXP;
        public const string ASSET_FOLDER = "Assets";
        public const string RESOURCES_PATH = "Assets/Vurbiri/Scripts/Editor/Localization/Resources/";

        public const string LANGS = "Languages";

        public const string SETTINGS_PATH = GLOBAL_CONST.RESOURCES_PATH;
        public const string SETTINGS_NAME = "LocalizationSettings";
        public const string SETTINGS_FULL_PATH = SETTINGS_PATH + SETTINGS_NAME + "." + ASSET_EXP;

        public const string PROJECT_SETTINGS_NAME = GLOBAL_CONST.PROJECT_SETTINGS_NAME;
        public const string PROJECT_SETTINGS_PATH = GLOBAL_CONST.PROJECT_SETTINGS_PATH;

        public const string PROJECT_LABEL = "Localization";
        public const string PROJECT_MENU = "Project/Localization";

        public const string LANG_TYPES_NAME = "LanguageTypes";
        public const string LANG_TYPES_PATH = RESOURCES_PATH + LANG_TYPES_NAME + "." + ASSET_EXP;

        public const string LANG_PROJECT_LABEL = "Languages";
        public const string LANG_PROJECT_MENU = PROJECT_MENU + "/Languages";

        public const string STR_LANG_NAME = "LanguageStrings";
        public const string STR_LANG_PATH = RESOURCES_PATH + STR_LANG_NAME + "." + ASSET_EXP;

        public const string STR_PROJECT_LABEL = "Strings";
        public const string STR_PROJECT_MENU = PROJECT_MENU + "/Strings";
    }
}
