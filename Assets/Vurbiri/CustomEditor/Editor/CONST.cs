using UnityEngine;

namespace VurbiriEditor
{
    internal static class CONST
    {
        public const string MENU_PATH = "Vurbiri/";

        public const string LABEL_COLOR = "Color";
        
        public static readonly string[] labelsSpecular = { "Metallic", "Smoothness" };
        public const int COUNT_SPECULAR = 2;
        public static readonly GUIContent labelColor = new(LABEL_COLOR), labelEmpty = new();
    }
}
