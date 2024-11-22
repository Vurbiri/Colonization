//Assets\Colonization\Editor\Players\PlayerVisualSetEditor.cs
using UnityEditor;
using UnityEngine.UIElements;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(PlayerVisualSetScriptable), true), CanEditMultipleObjects]
    public class PlayerVisualSetEditor : AEditorGetVE<PlayerVisualSetEditor>
    {
        public override VisualElement CreateInspectorGUI() => CreateDefaultInspectorGUI();
    }
}
