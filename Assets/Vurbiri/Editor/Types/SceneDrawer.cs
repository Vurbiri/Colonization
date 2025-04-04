//Assets\Vurbiri\Editor\Types\SceneDrawer.cs
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(SceneId))]
    [CustomPropertyDrawer(typeof(LoadScene))]
    public class SceneDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_scene";

        private static string[] nameScenes;
        private static int[] idScenes;

        SerializedProperty _sceneProperty;

        static SceneDrawer()
        {
            CreateListScenes();

            EditorBuildSettings.sceneListChanged -= CreateListScenes;
            EditorBuildSettings.sceneListChanged += CreateListScenes;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _sceneProperty ??= property.FindPropertyRelative(NAME_VALUE);

            label = EditorGUI.BeginProperty(position, label, property);
            _sceneProperty.intValue = EditorGUI.IntPopup(position, label.text, _sceneProperty.intValue, nameScenes, idScenes);
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
