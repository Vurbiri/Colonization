//Assets\Colonization\Editor\Players\PlayerVisualSetWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PlayerVisualSetWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Player Visual", MENU = MENU_CH_PATH + NAME;
        #endregion

        [SerializeField] private PlayerVisualSetScriptable _visualSet;

        private Editor _editor;

        [MenuItem(MENU, false, 40)]
        private static void ShowWindow()
        {
            GetWindow<PlayerVisualSetWindow>(true, NAME).minSize = new(350f, 300f);
        }


        public void OnEnable()
        {
            _editor = Editor.CreateEditor(_visualSet);
        }


        public void OnGUI()
        {
            BeginWindows();
            EditorGUILayout.BeginVertical(GUI.skin.window);
            _editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            EndWindows();
        }

        public void OnDisable()
        {
            DestroyImmediate(_editor);
        }

        private void OnValidate()
        {
            if(_visualSet == null)
                _visualSet = Vurbiri.EUtility.FindAnyScriptable<PlayerVisualSetScriptable>();
        }

    }
}
