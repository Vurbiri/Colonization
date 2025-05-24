using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomEditor(typeof(HumanAbilitiesScriptable), true), CanEditMultipleObjects]
    internal class HumanAbilitiesEditor : AEditorGetVE<HumanAbilitiesEditor>
    {
        [SerializeField] private VisualTreeAsset _humanAbilitiesVT;

        public override VisualElement CreateInspectorGUI() => _humanAbilitiesVT.CloneTree();
    }
}
