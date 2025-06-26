using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Collections;
using Object = UnityEngine.Object;

namespace VurbiriEditor.Collections
{
    [CustomPropertyDrawer(typeof(IdSet<,>))]
    internal class IdHashSetDrawer : AIdCollectionDrawer
    {
        public readonly float BUTTON_RATE_POS = 0.33f, BUTTON_RATE_SIZE = 0.275f, LABEL_SIZE = 100f;
        public readonly string NAME_COUNT = "_count", LABEL_EMPTY = "-----";
        public readonly string BUTTON_CHILD = "Set children", BUTTON_PREFAB = "Set prefabs", BUTTON_ASSET = "Set assets", BUTTON_CLEAR = "Clear";

        public readonly Color colorNull = new(1f, 0.65f, 0f, 1f);
        public readonly Type typeObject = typeof(UnityEngine.Object), typeMono = typeof(MonoBehaviour);

        private int _countMax = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            position.y += _ySpace;

            Color prevColor = GUI.color;
            Rect startPosition = position;
            Type typeValue = fieldInfo.FieldType.GetGenericArguments()[INDEX_VALUE];
            SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
            SerializedProperty propertyCount = property.FindPropertyRelative(NAME_COUNT);

            SetPositiveNames();
            _countMax = _names.Length;

            label = EditorGUI.BeginProperty(position, label, property);
            {
                if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
                {
                    SerializedProperty propertyCurrent, propertyNull = null;

                    EditorGUI.indentLevel++;

                    for (int i = 0; i < _count; i++)
                    {
                        propertyCurrent = propertyValues.GetArrayElementAtIndex(i);
                        if (propertyCurrent.objectReferenceValue != null)
                        {
                            DrawField(propertyCurrent, _names[i]);
                            continue;
                        }

                        propertyNull = propertyCurrent;
                    }

                    if (propertyNull != null) DrawField(propertyNull, LABEL_EMPTY);
                    EditorGUI.indentLevel--;

                    if (!Application.isPlaying)
                    {
                        if (typeValue.Is(typeObject))
                        {
                            if (property.serializedObject.targetObject is Component && DrawButton(BUTTON_CHILD, 1))
                                GetComponentsInChildren(property.serializedObject.targetObject as Component);
                            if (typeValue.Is(typeMono))
                            {
                                if (DrawButton(BUTTON_PREFAB, 0)) LoadPrefabs();
                            }
                            else
                            {
                                if (DrawButton(BUTTON_ASSET, 0)) LoadAssets();
                            }
                        }

                        if (DrawButton(BUTTON_CLEAR, 2))
                            Clear();
                    }
                }
                DrawLabelCount();
            }
            EditorGUI.EndProperty();

            #region Local: DrawField(...), DrawLabelCount(), DrawButton(), SetPropertyArray(...)
            //=================================
            void DrawField(SerializedProperty prop, string name)
            {
                position.y += _height;
                if (prop.objectReferenceValue == null)
                    GUI.color = colorNull;
                EditorGUI.PropertyField(position, prop, new GUIContent(name));
                GUI.color = prevColor;
            }
            //=================================
            void DrawLabelCount()
            {
                GUIStyle style = new(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleRight
                };

                startPosition.x = EditorGUIUtility.currentViewWidth - LABEL_SIZE - 20f;
                startPosition.width = LABEL_SIZE;

                if (propertyCount.intValue < _countMax)
                    GUI.color = colorNull;
                EditorGUI.LabelField(startPosition, $"{propertyCount.intValue} / {_countMax}", style);
                GUI.color = prevColor;
            }
            //=================================
            bool DrawButton(string text, int spot)
            {
                Rect positionButton = position;
                float viewWidth = EditorGUIUtility.currentViewWidth;

                positionButton.height += _ySpace;
                positionButton.y += _height + _ySpace;
                positionButton.x = viewWidth * (BUTTON_RATE_POS * (0.5f + spot) - BUTTON_RATE_SIZE * 0.5f);
                positionButton.width = viewWidth * BUTTON_RATE_SIZE;

                return GUI.Button(positionButton, text.ToUpper());
            }
            //=================================
            void GetComponentsInChildren(Component component)
            {
                List<Object> list = new();

                Find(component.transform);
                SetValues(list);

                #region Local: Find(...)
                //=================================
                void Find(Transform parent)
                {
                    Object obj;
                    foreach (Transform child in parent)
                    {
                        obj = child.GetComponent(typeValue);
                        if (obj != null)
                            list.Add(obj);

                        Find(child);
                    }
                }
                #endregion
            }
            //=================================
            List<Object> LoadPrefabs()
            {
                string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
                string path;
                Object obj;
                List<Object> list = new();

                foreach (var guid in guids)
                {
                    path = AssetDatabase.GUIDToAssetPath(guid);
                    obj = (AssetDatabase.LoadMainAssetAtPath(path) as GameObject).GetComponent(typeValue);
                    if (obj != null)
                        list.Add(obj);
                }
                return list;
            }
            //=================================
            List<Object> LoadAssets()
            {
                string[] guids = AssetDatabase.FindAssets($"t:{typeValue.Name}", new[] { "Assets" });
                Object obj;
                List<Object> list = new();

                foreach (var guid in guids)
                {
                    obj = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid));
                    //if (obj.GetType().Is(typeValue))
                    list.Add(obj);
                }
                return list;
            }
            //=================================
            void SetValues(IReadOnlyList<Object> array)
            {
                for (int index = 0; index < array.Count; index++)
                    propertyValues.GetArrayElementAtIndex(index % _count).objectReferenceValue = array[index];

                propertyCount.intValue = array.Count;
            }
            //=================================
            void Clear()
            {
                for (int index = 0; index < _count; index++)
                    propertyValues.GetArrayElementAtIndex(index).objectReferenceValue = null;

                propertyCount.intValue = 0;
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1f;

            if (property.isExpanded)
            {
                float countCurrent = property.FindPropertyRelative(NAME_COUNT).intValue;
                if (countCurrent < _countMax)
                    countCurrent += 1f;

                rate += countCurrent;
                if (!Application.isPlaying)
                    rate += 1.3f;
            }

            return _height * rate;
        }
    }
}
