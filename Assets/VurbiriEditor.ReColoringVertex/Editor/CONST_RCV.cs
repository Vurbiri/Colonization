using System;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    internal static class CONST_RCV
    {
        public const string MENU_PATH = "Vurbiri/";
        public const string PALETTE_PATH = "Assets/VurbiriEditor.ReColoringVertex/Editor/Resources/";
        public const string ICON = "Assets/VurbiriEditor.ReColoringVertex/Editor/Icons/IconEditorPalette.png";
        public const string LABEL_COLOR = "Color";
        public static readonly string[] labelsSpecular = { "Metallic", "Smoothness" };
        public const int COUNT_SPECULAR = 2;
        public static readonly GUIContent labelColor = new(LABEL_COLOR), labelEmpty = new();

        public const string PALETTE_EXP = "asset", MESH_EXP = "mesh";

        public const string LABEL_IS_INVERT = " Invert order of SubMesh", LABEL_EDIT_NAMES = " Edit names";

        public const int SPACE_WND = 8;

        public static readonly Type typeMesh = typeof(Mesh), typePalette = typeof(PaletteVertexScriptable);

        public static readonly Vector2 wndMinSize = new(450f, 425f);

        
    }
}
