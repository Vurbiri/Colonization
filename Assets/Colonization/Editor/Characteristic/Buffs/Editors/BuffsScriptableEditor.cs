using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(BuffsScriptable), true), CanEditMultipleObjects]
	public class BuffsScriptableEditor : Editor
	{
		public const string F_MAX_LEVEL = "_maxLevel";
        public const string F_SETTINGS = "_settings";

        private SerializedProperty _maxLevelProperty;
        private SerializedProperty _settingsProperty;
		
		private void OnEnable()
		{
            _maxLevelProperty = serializedObject.FindProperty(F_MAX_LEVEL);
            _settingsProperty = serializedObject.FindProperty(F_SETTINGS);		
		}
		
		public override void OnInspectorGUI()
		{
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(_maxLevelProperty);
            EditorGUILayout.PropertyField(_settingsProperty);
			EditorGUI.EndDisabledGroup();
        }
	}
}
