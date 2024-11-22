//Assets\Colonization\Editor\Actors\Utility\Window\AnimationClipSettingsWindow.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    public class AnimationClipSettingsWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _treeAnimationClipSettingsWindow;

        private const string NAME = "Animation Clips Settings", MENU = MENU_PATH + NAME;
        private const string NAME_CONTAINER = "Container";

        private readonly List<Editor> _editors = new();
        private static readonly Vector2 wndMinSize = new(325f, 500f);

        [MenuItem(MENU)]
        public static void ShowWindow()
        {
            GetWindow<AnimationClipSettingsWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            List<AnimationClipSettingsScriptable> settings = Utility.FindScriptables<AnimationClipSettingsScriptable>();

            if (settings == null || settings.Count == 0 || _treeAnimationClipSettingsWindow == null)
                return;

            var root = _treeAnimationClipSettingsWindow.CloneTree();
            var container = root.Q<VisualElement>(NAME_CONTAINER);

            for(int i = settings.Count - 1; i >= 0; i--)
            {
                container.Add(AnimationClipSettingsEditor.CreateEditorAndBind(settings[i], out Editor editor));
                _editors.Add(editor);
            }

            rootVisualElement.Add(root);
        }

        private void OnDisable()
        {
            foreach (var editor in _editors)
                DestroyImmediate(editor);

            _editors.Clear();
        }
    }
}
