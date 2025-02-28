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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneProperty = property.FindPropertyRelative(NAME_VALUE);
            int countScenes = EditorBuildSettings.scenes.Length;
            string[] nameScenes = new string[countScenes];
            int[] idScenes = new int[countScenes];

            for (int i = 0; i < countScenes; i++)
            {
                nameScenes[i] = $"{Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path)} ({i})";
                idScenes[i] = i;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            sceneProperty.intValue = EditorGUI.IntPopup(position, label.text, sceneProperty.intValue, nameScenes, idScenes);
            EditorGUI.EndProperty();
        }
    }
}
