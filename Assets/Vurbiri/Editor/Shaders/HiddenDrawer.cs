using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public class HiddenDrawer : MaterialPropertyDrawer
    {
        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
