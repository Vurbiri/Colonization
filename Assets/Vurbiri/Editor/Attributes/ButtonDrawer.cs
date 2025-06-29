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

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, mainProperty, label, true);
            if (attribute is ButtonAttribute button)
            {
                position.y += _height;
                object target = mainProperty.serializedObject.targetObject;
                if ((target != _target || _methodName != button.methodName || _action == null) && !TryCreateDelegate(target, button.methodName))
                {
                    EditorGUI.HelpBox(position, $"Failed to retrieve method {button.methodName}", UnityEditor.MessageType.Error);
                    return;
                }
                if (GUI.Button(position, button.methodName))
                    _action();
            }
        }

        public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(mainProperty); ;
            if (attribute is ButtonAttribute)
                height += _height;

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