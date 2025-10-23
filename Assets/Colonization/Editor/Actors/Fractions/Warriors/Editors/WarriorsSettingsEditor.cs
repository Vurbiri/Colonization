using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(WarriorsSettingsScriptable), true)]
    public class WarriorsSettingsEditor : ActorsSettingsEditor<WarriorsSettingsEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<WarriorId>("Warrior Settings");
    }
}
