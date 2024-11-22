//Assets\Colonization\Editor\Actors\Warriors\Editors\WarriorsSettingsEditor.cs
namespace VurbiriEditor.Colonization.Actors
{
    using UnityEditor;
    using UnityEngine.UIElements;
    using Vurbiri.Colonization.Actors;

    [CustomEditor(typeof(WarriorsSettingsScriptable), true)]
    public class WarriorsSettingsEditor : AActorsSettingsEditor<WarriorsSettingsEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateGUI<WarriorId>("Warrior Settings");
    }
}
