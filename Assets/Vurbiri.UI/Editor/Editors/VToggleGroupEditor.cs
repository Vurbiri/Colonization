//Assets\Vurbiri.UI\Editor\Editors\VToggleGroupEditor.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleGroup), true)]
	public class VToggleGroupEditor : Editor
	{
        private const int ORDER = 12;

        private SerializedProperty _allowSwitchOffProperty;
		private VToggleGroup _toggleGroup;

        private void OnEnable()
		{
			_allowSwitchOffProperty = serializedObject.FindProperty("_allowSwitchOff");
            _toggleGroup = target as VToggleGroup;

            MonoScript monoScript = MonoScript.FromMonoBehaviour(_toggleGroup);
            if (monoScript != null && MonoImporter.GetExecutionOrder(monoScript) != ORDER)
                MonoImporter.SetExecutionOrder(monoScript, ORDER);
        }
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Space(4f);
            EditorGUI.BeginChangeCheck();
			PropertyField(_allowSwitchOffProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(_toggleGroup.gameObject.scene);

                _toggleGroup.allowSwitchOff = _allowSwitchOffProperty.boolValue;
            }

            Space(2f);

            serializedObject.ApplyModifiedProperties();
		}
	}
}