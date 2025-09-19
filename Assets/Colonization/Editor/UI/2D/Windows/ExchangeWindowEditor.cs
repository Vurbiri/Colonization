using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace VurbiriEditor.Colonization.UI
{
	[CustomEditor(typeof(ExchangeWindow)), CanEditMultipleObjects]
	public class ExchangeWindowEditor : Editor
	{
        private ExchangeWindow _exchangeWindow;

        private void OnEnable()
		{
            _exchangeWindow = (ExchangeWindow)target;
        }
		
		public override void OnInspectorGUI()
		{
            base.OnInspectorGUI();
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Setup"))
                _exchangeWindow.Setup_Editor();
            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
                _exchangeWindow.Create_Editor();
            if (GUILayout.Button("Delete"))
                _exchangeWindow.Delete_Editor();
        }
	}
}