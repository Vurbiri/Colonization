using System;
using UnityEditor;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AGetComponentAttributeDrawer : PropertyDrawer
    {
        private const float BUTTON_SIZE = 35f, BUTTON_POS = BUTTON_SIZE * 1.1f;
        private const string BUTTON_TEXT = "Set";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type typeProperty = fieldInfo.FieldType;

            if (!Application.isPlaying && DrawButton())
            {
                MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
                SetProperty(property, mono.gameObject, typeProperty);
            }

            EditorGUI.PropertyField(position, property, label);

            #region Local: DrawButton()
            //=================================
            bool DrawButton()
            {
                Rect positionButton = position;

                position.width -= BUTTON_POS;
                positionButton.x = EditorGUIUtility.currentViewWidth - BUTTON_POS;
                positionButton.width = BUTTON_SIZE;

                return GUI.Button(positionButton, BUTTON_TEXT.ToUpper());
            }
            #endregion
        }

        protected abstract void SetProperty(SerializedProperty property, GameObject gameObject, Type type);


    }
}
