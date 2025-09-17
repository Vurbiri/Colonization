using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        private object _target;
        private string _methodName;
        private Action _action;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var button = (ButtonAttribute)attribute;
            object target = mainProperty.serializedObject.targetObject;

            if (button.drawProperty)
            {
                EditorGUI.PropertyField(position, mainProperty, label, true);
                position.y += EditorGUI.GetPropertyHeight(mainProperty) + EditorGUIUtility.standardVerticalSpacing;
            }

            if ((target != _target | _methodName != button.methodName | _action == null) && !TryCreateDelegate(target, button.methodName))
            {
                EditorGUI.HelpBox(position, $"Failed to retrieve method \"{button.methodName}\"", UnityEditor.MessageType.Error);
            }
            else
            {
                float offset = position.width * 0.125f;
                position.width -= offset * 2f; position.x += offset;
                if (GUI.Button(position, button.caption))
                    _action();
            }
        }

        public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
        {
            var button = (ButtonAttribute)attribute;
            float height = 0f;

            if (button.drawProperty)
                height = EditorGUI.GetPropertyHeight(mainProperty);
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }

        public bool TryCreateDelegate(object target, string methodName)
        {
            if (target == null | string.IsNullOrEmpty(methodName))
                return false;

            _target = target; _methodName = methodName;

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            MethodInfo method = target.GetType().GetMethod(methodName, flags, null, new Type[0], null);
            if (method == null)
                return false;

            _action = Delegate.CreateDelegate(typeof(Action), method.IsStatic ? null : target, method) as Action;
            return _action != null;
        }
    }
}