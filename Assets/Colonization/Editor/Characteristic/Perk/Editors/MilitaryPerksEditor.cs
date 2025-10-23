using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(PerksScriptable), true), CanEditMultipleObjects]
    public class MilitaryPerksEditor : APerksEditor<MilitaryPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<MilitaryPerksId>("Military Perks");
    }
}
