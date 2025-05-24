using UnityEditor;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleGroup))]
	public class VToggleGroupEditor : Editor
	{
        private const int EXECUTION_ORDER = VUI_CONST_ED.TOGGLE_GROUP_EXECUTION_ORDER;

        private SerializedProperty _allowSwitchOffProperty;

        private VToggleGroup _toggleGroup;

        private void OnEnable()
		{
            _toggleGroup = target as VToggleGroup;

            _allowSwitchOffProperty = serializedObject.FindProperty("_allowSwitchOff");

            MonoScript monoScript = MonoScript.FromMonoBehaviour(_toggleGroup);
            if (monoScript != null && MonoImporter.GetExecutionOrder(monoScript) != EXECUTION_ORDER)
                MonoImporter.SetExecutionOrder(monoScript, EXECUTION_ORDER);
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
