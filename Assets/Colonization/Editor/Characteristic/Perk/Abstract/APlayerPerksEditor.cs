//Assets\Colonization\Editor\Characteristic\Perk\Abstract\APlayerPerksEditor.cs
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization.Characteristics
{
    public abstract class APlayerPerksEditor<T> : AEditorGetVE<T> where T : APlayerPerksEditor<T>
    {
        [SerializeField] private VisualTreeAsset _treePerksVT;
        [SerializeField] private VisualTreeAsset _treePerkVT;

        #region Consts
        private const string P_PERKS = "_perks", P_ARRAY = "_values", P_ID = "_id";
        private const string P_LEVEL = "_level", P_TARGET_OBJ = "_targetObject", P_TARGET_AB = "_targetAbility", P_TYPE_OP = "_typeModifier";
        private const string P_POS = "_position", P_SPRITE = "_sprite", P_KEY_DESC = "_keyDescription";
        private const string P_TYPE = "_type", P_VALUE = "_value", P_COST = "_cost";
        private const string U_CONTAINER = "Container", U_LABEL = "Label";
        private const string PREFF_KEY_DESC = "Perk";
        #endregion

        protected VisualElement CreateGUI<TId>(string captionText) where TId : APerkId<TId>
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
                element.Q<IMGUIContainer>(U_CONTAINER).onGUIHandler = () => IMGUIPerk<TId>(id, propertyPerk);

                container.Add(element);
            }

            return root;
        }

        private void IMGUIPerk<TId>(int id, SerializedProperty propertyPerk) 
            where TId : APerkId<TId>
        {
            int minLevel = 0, minPosition = 0, minCost = 1, maxLevel = 6, maxPosition = 7, maxCost = 16;
            Type type = typeof(TId);

            serializedObject.Update();

            propertyPerk.FindPropertyRelative(P_ID).intValue = id;
            propertyPerk.FindPropertyRelative(P_TYPE).intValue = type == typeof(EconomicPerksId) ? TypePerksId.Economic : TypePerksId.Military;

            BeginVertical(GUI.skin.window);
            indentLevel++;

            int target = DrawId(P_TARGET_OBJ, typeof(TargetOfPerkId));
            
            Space();
            DrawIntSlider(P_LEVEL, minLevel, maxLevel);
            DrawIntSlider(P_COST, minCost, maxCost);
            
            Space();
            int ability = DrawId(P_TARGET_AB, target == TargetOfPerkId.Player ? typeof(HumanAbilityId) : typeof(ActorAbilityId));
            DrawValue(target, ability);
            DrawId(P_TYPE_OP, typeof(TypeModifierId));

            Space();
            DrawIntSlider(P_POS, minPosition, maxPosition);
            DrawDesc();
            SerializedProperty property = propertyPerk.FindPropertyRelative(P_SPRITE);
            property.objectReferenceValue = ObjectField(property.displayName, property.objectReferenceValue, typeof(Sprite), false);

            indentLevel--;
            Space();
            EndVertical();

            serializedObject.ApplyModifiedProperties();

            #region Local: DrawIntSlider(..), DrawId(..), DrawValue(..), DrawDesc(..)
            //================================================================
            int DrawIntSlider(string nameProperty, int min, int max)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                int value = Mathf.Clamp(property.intValue, min, max);
                property.intValue = value = IntSlider(property.displayName, value, min, max);
                return value;
            }
            //================================================================
            int DrawId(string nameProperty, Type t_field, bool isNone = false, int miss = -1)
            {
                var (names, values) = GetNamesAndValues(t_field, isNone, miss);
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                property.intValue = IntPopup(property.displayName, property.intValue, names, values);
                return property.intValue;
            }
            //================================================================
            void DrawValue(int target, int ability)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_VALUE);
                if(target == TargetOfPerkId.Warriors & ability <= ActorAbilityId.MAX_RATE_ABILITY)
                    property.intValue = IntField(property.displayName, property.intValue / ActorAbilityId.RATE_ABILITY) * ActorAbilityId.RATE_ABILITY;
                else
                    property.intValue = IntField(property.displayName, property.intValue);
            }
            //================================================================
            void DrawDesc()
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_KEY_DESC);

                if (string.IsNullOrEmpty(property.stringValue))
                    property.stringValue = PREFF_KEY_DESC.Concat(APerkId<TId>.GetName(id));

                property.stringValue = TextField(property.displayName, property.stringValue);
                Space();
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
