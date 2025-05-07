//Assets\Colonization\Editor\Utility\Abstract\ASettingsWindow.cs
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public abstract class ASettingsWindow<T> : EditorWindow
	{
        [SerializeField] private T _settings;

        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;

        protected abstract string Caption { get; }

        private void OnEnable()
		{
            SettingsFileEditor.Load(ref _settings);
            _serializedObject = new(this);
            _serializedProperty = _serializedObject.FindProperty("_settings");
        }
		
		private void OnGUI()
		{
            _serializedObject.Update();
            BeginWindows();
            {
                Space(10f);
                LabelField(Caption, STYLES.H1);

                BeginVertical(GUI.skin.box);
                    PropertyField(_serializedProperty);
                EndVertical();
            }
            EndWindows();
            _serializedObject.ApplyModifiedProperties();
        }
		
		private void OnDisable()
		{
            SettingsFileEditor.Save(_settings);
        }
	}
}