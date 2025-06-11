using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(ABuffsScriptable<>), true), CanEditMultipleObjects]
	public class ABuffsScriptableEditor : Editor
	{
		protected SerializedProperty _serializedProperty;
		
		private void OnEnable()
		{
			_serializedProperty = serializedObject.FindProperty("_settings");		
		}
		
		public override void OnInspectorGUI()
		{
            EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(_serializedProperty);
			EditorGUI.EndDisabledGroup();
        }
	}
}
