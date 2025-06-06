using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomEditor(typeof(PerksScriptable), true)]
    internal class EconomicPerksEditor : APerksEditor<EconomicPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<EconomicPerksId>("Economic Perks");
    }
}
