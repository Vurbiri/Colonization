using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(DemonsSettingsScriptable), true)]
    public class DemonsSettingsEditor : ActorsSettingsEditor<DemonsSettingsEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<DemonId>("Demon Settings");
    }
}
