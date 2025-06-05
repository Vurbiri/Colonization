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
        private const string P_PERKS = "_perks";
        private const string P_ID = "_id", P_LEVEL = "_level", P_TARGET_OBJ = "_targetObject", P_TARGET_AB = "_targetAbility", P_TYPE_OP = "_typeModifier";
        private const string P_POS = "_position", P_SPRITE = "_sprite", P_KEY_DESC = "_keyDescription";
        private const string P_TYPE = "_type", P_VALUE = "_value", P_COST = "_cost";
        private const string U_CONTAINER = "Container", U_LABEL = "Label";
        private const string PREFF_KEY_DESC = "Perk";
        #endregion

        private readonly string[][] _ability = { HumanAbilityId.Names, ActorAbilityId.Names };
        private readonly NamesAndValues _modifier = new(TypeModifierId.Names, TypeModifierId.Values);

        protected VisualElement CreateGUI<TId>(string captionText) where TId : APerkId<TId>
        {
            var root = _treePerksVT.CloneTree();
            root.Q<Label>(U_LABEL).text = captionText;
            var container = root.Q<VisualElement>(U_CONTAINER);

            SerializedProperty propertyPerks = serializedObject.FindProperty(P_PERKS);
            propertyPerks.arraySize = APerkId<TId>.Count;
            serializedObject.ApplyModifiedProperties();

            VisualElement element;
            for (int i = 0; i < APerkId<TId>.Count; i++)
            {
                int id = i;
                SerializedProperty propertyPerk = propertyPerks.GetArrayElementAtIndex(i);
                string name = APerkId<TId>.GetName(i);

                element = _treePerkVT.Instantiate(propertyPerk.propertyPath);
                element.Q<Label>(U_LABEL).text = name;
                element.Q<IMGUIContainer>(U_CONTAINER).onGUIHandler = () => IMGUIPerk<TId>(id, propertyPerk, name);

                container.Add(element);
            }

            return root;
        }

        private void IMGUIPerk<TId>(int id, SerializedProperty propertyPerk, string name) 
            where TId : APerkId<TId>
        {
            Type type = typeof(TId);

            serializedObject.Update();

            propertyPerk.FindPropertyRelative(P_ID).intValue = id;
            propertyPerk.FindPropertyRelative(P_TYPE).intValue = type == typeof(EconomicPerksId) ? TypePerksId.Economic : TypePerksId.Military;

            int target = -1, ability = -1;
            string abilityName = name.Split('_')[0];
            for (int i = 0; i < TargetOfPerkId.Count; i++)
            {
               if((ability = Array.IndexOf(_ability[i], abilityName)) >= 0)
               {
                    target = i; break;
               }
            }

            if (target < 0 | ability < 0)
            {
                HelpBox($"Не верное имя {name}", UnityEditor.MessageType.Error);
                return;
            }

            DrawEndSet(P_TARGET_OBJ, target, TargetOfPerkId.Names[target]);
            DrawEndSet(P_TARGET_AB, ability, _ability[target][ability]);
           
            DrawValue(target, ability);
            DrawId(P_TYPE_OP, _modifier);

            Space();
            int level = DrawIntSlider(P_LEVEL, PerkTree.MIN_LEVEL, PerkTree.MAX_LEVEL);
            propertyPerk.FindPropertyRelative(P_COST).intValue = level + 1;

            DrawPosition(P_POS, level, PerkTree.MIN_LEVEL, PerkTree.MAX_LEVEL);
            Space();
            DrawDesc(name);
            SerializedProperty property = propertyPerk.FindPropertyRelative(P_SPRITE);
            property.objectReferenceValue = ObjectField(property.displayName, property.objectReferenceValue, typeof(Sprite), false);

            serializedObject.ApplyModifiedProperties();

            #region Local: DrawIntSlider(..), DrawId(..), DrawValue(..), DrawDesc(..)
            //================================================================
            void DrawEndSet(string nameProperty, int value, string text)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                property.intValue = value;
                LabelField(property.displayName, text);
            }
            //================================================================
            int DrawIntSlider(string nameProperty, int min, int max)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                int value = Mathf.Clamp(property.intValue, min, max);
                property.intValue = value = IntSlider(property.displayName, value, min, max);
                return value;
            }
            //================================================================
            void DrawPosition(string nameProperty, int level, int min, int max)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                int value = IntSlider(property.displayName, Mathf.Clamp((int)property.vector2Value.y, min, max), min, max);
                property.vector2Value = new(level, value);            
            }
            //================================================================
            int DrawId(string nameProperty, NamesAndValues types)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                property.intValue = IntPopup(property.displayName, property.intValue, types.names, types.values);
                return property.intValue;
            }
            //================================================================
            void DrawValue(int target, int ability)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_VALUE);
                if(target == TargetOfPerkId.Warriors & ability <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                    property.intValue = IntField(property.displayName, property.intValue >> ActorAbilityId.SHIFT_ABILITY) << ActorAbilityId.SHIFT_ABILITY;
                else
                    property.intValue = IntField(property.displayName, property.intValue);
            }
            //================================================================
            void DrawDesc(string name)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_KEY_DESC);

                if (string.IsNullOrEmpty(property.stringValue))
                    property.stringValue = PREFF_KEY_DESC.Concat(name);

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

        private readonly struct NamesAndValues
        {
            public readonly string[] names;
            public readonly int[] values;

            public NamesAndValues(string[] names, int[] values)
            {
                this.names = names;
                this.values = values;
            }
        }
    }
}
