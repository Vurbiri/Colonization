//Assets\Vurbiri\Editor\ReColoringVertex\CONST_RCV.cs
using System;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    internal static class CONST_RCV
    {
        public const string MENU_PATH = VurbiriEditor.CONST_EDITOR.MENU_PATH;
        public const string PALETTE_PATH = "Assets/Vurbiri/ReColoringVertex/Editor/Resources/";

        public const string PALETTE_EXP = "asset", MESH_EXP = "mesh";

        public const string LABEL_IS_INVERT = " Invert order of SubMesh", LABEL_EDIT_NAMES = " Edit names";

        public const int SPACE_WND = 8;

        public static readonly Type typeMesh = typeof(Mesh), typePalette = typeof(PaletteVertexScriptable);

        public static readonly Vector2 wndMinSize = new(450f, 425f);
    }
}
