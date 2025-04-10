//Assets\Vurbiri.UI\Editor\Editors\VToggleGroupEditor.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleGroup), true)]
	public class VToggleGroupEditor : Editor
	{
        private const int ORDER = 12;
        private static readonly string _name = "Allow Switch Off";

		private VToggleGroup _toggleGroup;
        private bool _allowSwitchOff;

        private void OnEnable()
		{
            _toggleGroup = target as VToggleGroup;
            
            MonoScript monoScript = MonoScript.FromMonoBehaviour(_toggleGroup);
            if (monoScript != null && MonoImporter.GetExecutionOrder(monoScript) != ORDER)
                MonoImporter.SetExecutionOrder(monoScript, ORDER);
        }
		
		public override void OnInspectorGUI()
		{
            serializedObject.ApplyModifiedProperties();

            Space(1f);
            EditorGUI.BeginChangeCheck();
            _allowSwitchOff = Toggle(_name, _toggleGroup.allowSwitchOff);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggleGroup.gameObject.scene);

                _toggleGroup.allowSwitchOff = _allowSwitchOff;
            }

            Space(1f);

            serializedObject.Update();
		}
	}
}