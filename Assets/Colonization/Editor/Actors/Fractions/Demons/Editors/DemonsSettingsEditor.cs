using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    [CustomEditor(typeof(DemonsSettingsScriptable), true)]
    public class DemonsSettingsEditor : ActorsSettingsEditor<DemonsSettingsEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<DemonId>("Demon Settings");
    }
}
