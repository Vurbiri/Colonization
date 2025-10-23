using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(HumanAbilitiesScriptable), true), CanEditMultipleObjects]
    internal class HumanAbilitiesEditor : AEditorGetVE<HumanAbilitiesEditor>
    {
        [SerializeField] private VisualTreeAsset _humanAbilitiesVT;

        public override VisualElement CreateInspectorGUI() => _humanAbilitiesVT.CloneTree();
    }
}
