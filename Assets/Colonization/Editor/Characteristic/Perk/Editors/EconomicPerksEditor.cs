//Assets\Colonization\Editor\Characteristic\Perk\Editors\EconomicPerksEditor.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine.UIElements;
    using Vurbiri.Colonization.Characteristics;

    [CustomEditor(typeof(EconomicPerksScriptable), true), CanEditMultipleObjects]
    internal class EconomicPerksEditor : APlayerPerksEditor<EconomicPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI("Economic Perks", target as EconomicPerksScriptable);
    }
}
