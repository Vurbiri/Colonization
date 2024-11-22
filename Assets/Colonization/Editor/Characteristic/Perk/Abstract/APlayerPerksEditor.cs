//Assets\Colonization\Editor\Characteristic\Perk\Abstract\APlayerPerksEditor.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Vurbiri;
    using Vurbiri.Colonization;
    using Vurbiri.Colonization.Characteristics;
    using VurbiriEditor;

    public abstract class APlayerPerksEditor<T> : AEditorGetVE<T> where T : APlayerPerksEditor<T>
    {
        [SerializeField] private VisualTreeAsset _treePerksVT;
        [SerializeField] private VisualTreeAsset _treePerkVT;

        #region Consts
        private const string P_PERKS = "_perks", P_ARRAY = "_values", P_ID = "_id", P_TYPE = "_type";
        private const string P_LEVEL = "_level", P_TARGET_OBJ = "_targetObject", P_TARGET_AB = "_targetAbility", P_TYPE_OP = "_typeModifier";
        private const string P_POS = "_position", P_SPRITE = "_sprite", P_KEY_DESC = "_keyDescription";
        private const string P_VALUE = "_value", P_CHANCE = "_chance", P_PREV = "_prevPerk";
        private const string U_CONTAINER = "Container", U_LABEL = "Label";
        private const string PREFF_KEY_DESC = "Perk";
        private const int SPACE_WND = 4;
        #endregion

        protected VisualElement CreateGUI<TId>(string captionText, APlayerPerksScriptable<TId> perks) where TId : APerkId<TId>
        {
            var root = _treePerksVT.CloneTree();
            root.Q<Label>(U_LABEL).text = captionText;
            var container = root.Q<VisualElement>(U_CONTAINER);

            SerializedProperty propertyPerks = serializedObject.FindProperty(P_PERKS).FindPropertyRelative(P_ARRAY);

            VisualElement element;
            for (int i = 0; i < APerkId<TId>.Count; i++)
            {
                int id = i;
                SerializedProperty propertyPerk = propertyPerks.GetArrayElementAtIndex(i);
                
                element = _treePerkVT.Instantiate(propertyPerk.propertyPath);
                element.Q<Label>(U_LABEL).text = APerkId<TId>.GetName(i);
                element.Q<IMGUIContainer>(U_CONTAINER).onGUIHandler = () => IMGUIPerk<TId>(id, propertyPerk, propertyPerks, perks);

                container.Add(element);
            }

            return root;
        }

        private void IMGUIPerk<TId>(int id, SerializedProperty propertyPerk, SerializedProperty propertyPerks, APlayerPerksScriptable<TId> perks) 
            where TId : APerkId<TId>
        {
            SerializedProperty property;
            int prev, minL = 1, minP = 1, maxL = 6, maxP = 7;
            Type type = typeof(TId);

            serializedObject.Update();

            propertyPerk.FindPropertyRelative(P_ID).intValue = id;
            propertyPerk.FindPropertyRelative(P_TYPE).intValue = type == typeof(EconomicPerksId) ? TypePerksId.Economic : TypePerksId.Military;

            EditorGUILayout.BeginVertical(GUI.skin.window);
            EditorGUI.indentLevel++;
                        
            DrawId(P_TARGET_OBJ, typeof(TargetOfPerkId));
            DrawId(P_TARGET_AB, property.intValue == TargetOfPerkId.Player ? typeof(PlayerAbilityId) : typeof(ActorAbilityId));

            Space();
            DrawInt(P_VALUE);
            if (DrawId(P_TYPE_OP, typeof(TypeModifierId)) == TypeModifierId.RandomAdd)
            {
                EditorGUILayout.PropertyField(propertyPerk.FindPropertyRelative(P_CHANCE));
            }

            Space();
            if ((prev = DrawId(P_PREV, type, true, id)) >= 0)
            {
                SerializedProperty propertyPrevPerk = propertyPerks.GetArrayElementAtIndex(prev);
                minL = propertyPrevPerk.FindPropertyRelative(P_LEVEL).intValue + 1;
                minP = maxP = propertyPrevPerk.FindPropertyRelative(P_POS).intValue;
            }

            Space();
            perks.Perks[id].Cost.Set(CurrencyId.Mana, DrawIntSlider(P_LEVEL, minL, maxL));
            DrawIntSlider(P_POS, minP, maxP);

            Space(SPACE_WND << 1);
            property = propertyPerk.FindPropertyRelative(P_KEY_DESC);
            if (string.IsNullOrEmpty(property.stringValue))
                property.stringValue = PREFF_KEY_DESC.Concat(APerkId<TId>.GetName(id));
            property.stringValue = EditorGUILayout.TextField(property.displayName, property.stringValue);

            property = propertyPerk.FindPropertyRelative(P_SPRITE);
            property.objectReferenceValue = EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, typeof(Sprite), false);

            EditorGUI.indentLevel--;
            Space(SPACE_WND << 2);
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            #region Local: Space(..), DrawIntSlider(..), DrawId(..), DrawInt(..), DrawString(..)
            //================================================================
            void Space(int value = SPACE_WND) => EditorGUILayout.Space(value);
            //================================================================
            int DrawIntSlider(string nameProperty, int min, int max)
            {
                property = propertyPerk.FindPropertyRelative(nameProperty);
                int value = Mathf.Clamp(property.intValue, min, max);
                property.intValue = value = EditorGUILayout.IntSlider(property.displayName, value, min, max);
                return value;
            }
            //================================================================
            int DrawId(string nameProperty, Type t_field, bool isNone = false, int miss = -1)
            {
                var (names, values) = GetNamesAndValues(t_field, isNone, miss);
                property = propertyPerk.FindPropertyRelative(nameProperty);
                property.intValue = EditorGUILayout.IntPopup(property.displayName, property.intValue, names, values);
                return property.intValue;
            }
            //================================================================
            void DrawInt(string nameProperty)
            {
                property = propertyPerk.FindPropertyRelative(nameProperty);
                property.intValue = EditorGUILayout.IntField(property.displayName, property.intValue);
            }
            #endregion
        }

        private (string[] names, int[] values) GetNamesAndValues(Type t_field, bool isNone = false, int miss = -1)
        {
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);

            FieldInfo[] fields = t_field.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length;
            List<string> names = new(count);
            List<int> values = new(count);

            if (isNone)
            {
                names.Add("None");
                values.Add(-1);
            }

            FieldInfo field;
            for (int i = 0; i < count; i++)
            {
                field = fields[i];

                if (i == miss | field.FieldType != t_int | !field.IsLiteral || field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                names.Add(field.Name);
                values.Add((int)field.GetValue(null));
            }

            return (names.ToArray(), values.ToArray());
        }
    }
}
