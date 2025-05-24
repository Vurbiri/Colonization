using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization.Characteristics
{

    [CustomEditor(typeof(EconomicPerksScriptable), true), CanEditMultipleObjects]
    internal class EconomicPerksEditor : APlayerPerksEditor<EconomicPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<EconomicPerksId>("Economic Perks");
    }
}
