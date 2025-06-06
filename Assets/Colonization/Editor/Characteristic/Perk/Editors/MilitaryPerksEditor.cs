using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomEditor(typeof(PerksScriptable), true), CanEditMultipleObjects]
    public class MilitaryPerksEditor : APerksEditor<MilitaryPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<MilitaryPerksId>("Military Perks");
    }
}
