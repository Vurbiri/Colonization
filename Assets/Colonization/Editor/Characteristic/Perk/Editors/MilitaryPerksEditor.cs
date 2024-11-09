using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(MilitaryPerksScriptable), true), CanEditMultipleObjects]
    public class MilitaryPerksEditor : APlayerPerksEditor<MilitaryPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI("Military Perks", target as MilitaryPerksScriptable);
    }
}
