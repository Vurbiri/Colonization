using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(PerksScriptable), true)]
    public class EconomicPerksEditor : APerksEditor<EconomicPerksEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<EconomicPerksId>("Economic Perks");
    }
}
