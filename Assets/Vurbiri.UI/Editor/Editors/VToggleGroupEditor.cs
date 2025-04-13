//Assets\Vurbiri.UI\Editor\Editors\VToggleGroupEditor.cs
using UnityEditor;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleGroup))]
	public class VToggleGroupEditor : Editor
	{
        private const int ORDER = 12;

        private SerializedProperty _allowSwitchOffProperty;

        private VToggleGroup _toggleGroup;

        private void OnEnable()
		{
            _toggleGroup = target as VToggleGroup;

            _allowSwitchOffProperty = serializedObject.FindProperty("_allowSwitchOff");

            MonoScript monoScript = MonoScript.FromMonoBehaviour(_toggleGroup);
            if (monoScript != null && MonoImporter.GetExecutionOrder(monoScript) != ORDER)
                MonoImporter.SetExecutionOrder(monoScript, ORDER);
        }
		
		public override void OnInspectorGUI()
		{
            serializedObject.Update();

            EditorGUILayout.Space(1f);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_allowSwitchOffProperty);
            if (EditorGUI.EndChangeCheck())
                _toggleGroup.AllowSwitchOff = _allowSwitchOffProperty.boolValue;

            EditorGUILayout.Space(1f);

            serializedObject.ApplyModifiedProperties();
		}
	}
}