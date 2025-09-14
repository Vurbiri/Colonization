using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(SceneId))]
    public class SceneDrawer : PropertyDrawer
    {
        protected const string F_SCENE = "_scene";
        protected static string[] s_names;

        static SceneDrawer()
        {
            CreateListScenes();

            EditorBuildSettings.sceneListChanged -= CreateListScenes;
            EditorBuildSettings.sceneListChanged += CreateListScenes;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sceneProperty = property.FindPropertyRelative(F_SCENE);
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);
            {
                sceneProperty.intValue = EditorGUI.Popup(position, label.text, sceneProperty.intValue, s_names);
            }
            EditorGUI.EndProperty();
        }

        private static void CreateListScenes()
        {
            int count = EditorBuildSettings.scenes.Length;
            s_names = new string[count];
            for (int i = 0; i < count; i++)
                s_names[i] = $"{Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path)} ({i})";
        }
    }
}
