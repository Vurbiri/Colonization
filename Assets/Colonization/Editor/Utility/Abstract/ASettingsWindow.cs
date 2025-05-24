using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public abstract class ASettingsWindow<T> : EditorWindow
	{
        [SerializeField] private T _settings;

        private string _label;
        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;
        private Vector2 _scrollPos;

        private void OnEnable()
		{
            SettingsFileEditor.Load(ref _settings);
            _serializedObject = new(this);
            _serializedProperty = _serializedObject.FindProperty("_settings");
            _label = Regex.Replace(typeof(T).Name, "([a-z])([A-Z])", "$1 $2");
        }
		
		private void OnGUI()
		{
            _serializedObject.Update();
            BeginWindows();
            {
                Space(10f);
                LabelField(_label, STYLES.H1);

                BeginVertical(GUI.skin.box);
                    _scrollPos = BeginScrollView(_scrollPos);
                        PropertyField(_serializedProperty);
                    EndScrollView();
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
