//Assets\Colonization\Editor\Characteristic\Abilities\Editors\SatanAbilitiesEditor.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomEditor(typeof(SatanAbilitiesScriptable), true), CanEditMultipleObjects]
    public class SatanAbilitiesEditor : AEditorGetVE<SatanAbilitiesEditor>
    {
        [SerializeField] private VisualTreeAsset _satanAbilitiesVT;

        public override VisualElement CreateInspectorGUI() => _satanAbilitiesVT.CloneTree();
    }
}
