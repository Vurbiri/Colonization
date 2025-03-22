//Assets\Colonization\Editor\Characteristic\Perk\Editors\MilitaryPerksEditor.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine.UIElements;
    using Vurbiri.Colonization.Characteristics;

    [CustomEditor(typeof(MilitaryPerksScriptable), true), CanEditMultipleObjects]
    public class MilitaryPerksEditor : APlayerPerksEditor<MilitaryPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<MilitaryPerksId>("Military Perks");
    }
}
