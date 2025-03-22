//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(DiplomacySettingsScriptable), true), CanEditMultipleObjects]
	public class DiplomacySettingsEditor : Editor
	{
        private SerializedProperty _serializedProperty;
		
		private void OnEnable()
		{
			_serializedProperty = serializedObject.FindProperty("_settings");		
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Space(10);
			LabelField("Diplomacy Settings", STYLES.H1);

            BeginVertical(GUI.skin.box);
            PropertyField(_serializedProperty);
            EndVertical();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
