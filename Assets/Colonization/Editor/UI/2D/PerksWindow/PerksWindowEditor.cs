using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;
using VurbiriEditor.UI;

namespace VurbiriEditor.Colonization
{
	[CustomEditor(typeof(PerksWindow), true)]
	public class PerksWindowEditor : VToggleGroupEditor
    {
        private PerksWindow _perksWindow;

        protected override void OnEnable()
		{
			base.OnEnable();

            _perksWindow = (PerksWindow)target;

        }
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Setup"))
                _perksWindow.Setup_Editor();
            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
                _perksWindow.Create_Editor();
            if (GUILayout.Button("Delete"))
                _perksWindow.Delete_Editor();

        }
	}
}