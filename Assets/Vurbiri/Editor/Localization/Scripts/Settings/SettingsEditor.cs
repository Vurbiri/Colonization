//Assets\Vurbiri\Editor\Localization\Scripts\Settings\SettingsEditor.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.Localization.Editor
{
    [CustomEditor(typeof(SettingsScriptable), true)]
    internal class SettingsEditor : AEditorGetVE<SettingsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            _treeAsset.CloneTree(root);

            TextField filePath = root.Q<TextField>("FilePath");

            filePath.SetEnabled(false);

            return root;
        }
    }
}
