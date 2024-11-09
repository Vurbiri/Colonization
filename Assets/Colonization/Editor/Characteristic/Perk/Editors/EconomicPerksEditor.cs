using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(EconomicPerksScriptable), true), CanEditMultipleObjects]
    internal class EconomicPerksEditor : APlayerPerksEditor<EconomicPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI("Economic Perks", target as EconomicPerksScriptable);
    }
}
