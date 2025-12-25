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
        private static readonly float s_buttonWidth = 260f;
        private object _target;
        private string _methodName;
        private Action _action;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            var button = (ButtonAttribute)attribute;

            position.height = EditorGUIUtility.singleLineHeight;

            if (button.drawProperty)
            {
                EditorGUI.PropertyField(position, mainProperty, label, true);
                position.y += EditorGUI.GetPropertyHeight(mainProperty) + EditorGUIUtility.standardVerticalSpacing;
            }

            if (!TryCreateAction(mainProperty.serializedObject.targetObject, button.methodName))
            {
                EditorGUI.HelpBox(position, $"Failed to retrieve method \"{button.methodName}\"", UnityEditor.MessageType.Error);
            }
            else
            {
                position.x = (EditorGUIUtility.currentViewWidth - s_buttonWidth) * 0.5f + 15f;
                position.width = s_buttonWidth;
                if (GUI.Button(position, button.caption, EditorStyles.miniButtonMid))
                    _action();
            }

            // Local =======================================================
            bool TryCreateAction(object target, string methodName)
            {
                if (target == null | string.IsNullOrEmpty(methodName))
                    return false;

                if (target == _target & _methodName == methodName & _action != null)
                    return true;

                _target = target; _methodName = methodName;

                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                MethodInfo method = target.GetType().GetMethod(methodName, flags, null, new Type[0], null);
                if (method == null)
                    return false;

                _action = Delegate.CreateDelegate(typeof(Action), method.IsStatic ? null : target, method) as Action;
                return _action != null;
            }
        }

        public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
        {
            float height = ((ButtonAttribute)attribute).drawProperty ? EditorGUI.GetPropertyHeight(mainProperty) + EditorGUIUtility.standardVerticalSpacing : 0f;
            return height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}