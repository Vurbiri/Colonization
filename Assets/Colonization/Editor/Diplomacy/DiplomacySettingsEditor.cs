//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(DiplomacySettingsScriptable), true), CanEditMultipleObjects]
	public class DiplomacySettingsEditor : Editor
	{

		protected SerializedProperty _serializedProperty;
		
		private void OnEnable()
		{
			_serializedProperty = serializedObject.FindProperty("_settings");		
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.window);

            EditorGUILayout.PropertyField(_serializedProperty);

            EditorGUILayout.EndVertical();

			serializedObject.ApplyModifiedProperties();
		}
	}
}