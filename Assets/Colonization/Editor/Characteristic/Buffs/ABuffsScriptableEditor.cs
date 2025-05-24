using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(ABuffsScriptable<>), true), CanEditMultipleObjects]
	public class ABuffsScriptableEditor : Editor
	{
		#region Consts
		
		#endregion
		
		protected SerializedProperty _serializedProperty;
		
		private void OnEnable()
		{
			_serializedProperty = serializedObject.FindProperty("_settings");		
		}
		
		public override void OnInspectorGUI()
		{
            UnityEditor.EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.PropertyField(_serializedProperty);

            UnityEditor.EditorGUI.EndDisabledGroup();
        }
	}
}
