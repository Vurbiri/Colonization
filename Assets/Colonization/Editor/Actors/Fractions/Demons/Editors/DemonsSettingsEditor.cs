using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(DemonsSettingsScriptable), true)]
    public class DemonsSettingsEditor : ActorSettingsEditor<DemonsSettingsEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<DemonId, DemonSettings>("Demon Settings", ((DemonsSettingsScriptable)target).Settings);
    }
}
