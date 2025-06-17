using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleGroup<>), true)]
	public class VToggleGroupEditor : Editor
	{
        private const int EXECUTION_ORDER = VUI_CONST_ED.TOGGLE_GROUP_EXECUTION_ORDER;

        private SerializedProperty _allowSwitchOffProperty;
        private SerializedProperty _onValueChangedProperty;

        protected readonly List<SerializedProperty> _childrenProperties = new();
        protected bool _defaultDraw = true;

        protected virtual void OnEnable()
		{
            _allowSwitchOffProperty = serializedObject.FindProperty("_allowSwitchOff");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            MonoScript monoScript = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
            if (monoScript != null && MonoImporter.GetExecutionOrder(monoScript) != EXECUTION_ORDER)
                MonoImporter.SetExecutionOrder(monoScript, EXECUTION_ORDER);

            FindChildrenProperties();
        }
		
		public override void OnInspectorGUI()
		{
            serializedObject.Update();

            DrawChildrenProperties();

            Space(1f);
            PropertyField(_allowSwitchOffProperty);
            Space();
            PropertyField(_onValueChangedProperty);

            serializedObject.ApplyModifiedProperties();
		}

        private void DrawChildrenProperties()
        {
            if (_childrenProperties.Count == 0) return;

            Space(1f);
            BeginVertical(STYLES.borderDark);
            foreach (var child in _childrenProperties)
                PropertyField(child, true);
            EndVertical();
            Space();
        }

        private void FindChildrenProperties()
        {
            if (!_defaultDraw) return;

            HashSet<string> excludeProperties = GetExcludePropertyPaths();
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (!excludeProperties.Contains(iterator.name))
                    _childrenProperties.Add(iterator.Copy());
            }
        }

        protected virtual HashSet<string> GetExcludePropertyPaths()
        {
            return new(13)
            {
                serializedObject.FindProperty("m_Script").propertyPath,
                _allowSwitchOffProperty.propertyPath,
                _onValueChangedProperty.propertyPath
            };
        }
    }
}
