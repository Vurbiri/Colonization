using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    [CustomEditor(typeof(WarriorsSettingsScriptable), true)]
    public class WarriorsSettingsEditor : AActorsSettingsEditor<WarriorsSettingsEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<WarriorId>("Warrior Settings");
    }
}
