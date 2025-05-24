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

        protected static string[] nameScenes;
        protected static int[] idScenes;

        protected readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        static SceneDrawer()
        {
            CreateListScenes();

            EditorBuildSettings.sceneListChanged -= CreateListScenes;
            EditorBuildSettings.sceneListChanged += CreateListScenes;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneProperty = property.FindPropertyRelative(F_SCENE);
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);
            sceneProperty.intValue = EditorGUI.IntPopup(position, label.text, sceneProperty.intValue, nameScenes, idScenes);
            EditorGUI.EndProperty();
        }

        private static void CreateListScenes()
        {
            int countScenes = EditorBuildSettings.scenes.Length;
            nameScenes = new string[countScenes];
            idScenes = new int[countScenes];

            for (int i = 0; i < countScenes; i++)
            {
                nameScenes[i] = $"{Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path)} ({i})";
                idScenes[i] = i;
            }
        }
    }
}
