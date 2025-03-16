//Assets\Colonization\Editor\AdvGameMechanics\Balance\BalanceSettingsEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(BalanceSettingsScriptable), true), CanEditMultipleObjects]
	public class BalanceSettingsEditor : Editor
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
			LabelField("Balance Settings", STYLES.H1);

			BeginVertical(GUI.skin.box);
            PropertyField(_serializedProperty);
            Space(10);
           
            EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
	}
}