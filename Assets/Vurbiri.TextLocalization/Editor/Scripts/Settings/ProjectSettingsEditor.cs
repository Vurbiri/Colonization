//Assets\Vurbiri.TextLocalization\Editor\Scripts\Settings\ProjectSettingsEditor.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.TextLocalization.Editor
{
    [CustomEditor(typeof(ProjectSettingsScriptable), true)]
    internal class ProjectSettingsEditor : AEditorGetVE<ProjectSettingsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            return _treeAsset.CloneTree();
           
        }
    }
}
