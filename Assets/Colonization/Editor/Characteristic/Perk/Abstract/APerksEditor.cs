using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization.Characteristics
{
    public abstract class APerksEditor<T> : AEditorGetVE<T> where T : APerksEditor<T>
    {
        [SerializeField] private VisualTreeAsset _treePerksVT;
        [SerializeField] private VisualTreeAsset _treePerkVT;

        private readonly int MIN_LEVEL = PerkTree.MIN_LEVEL, MAX_LEVEL = PerkTree.MAX_LEVEL - 1;
        private readonly int MIN_POS = 0, MAX_POS = 5;

        private readonly string P_PERKS = "_perks", P_PERK_VALUES = "_values";
        private readonly string P_ID = "_id", P_LEVEL = "_level", P_TARGET_OBJ = "_targetObject", P_TARGET_AB = "_targetAbility";
        private readonly string P_TYPE = "_type", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_COST = "_cost";

        private readonly string P_PERK_MOD = "perkModifier", P_POS = "position", P_SPRITE = "sprite", P_KEY_DESC = "keyDescription";
        private readonly string PREFF_KEY_DESC = "Perk";

        private readonly string U_CONTAINER = "Container", U_LABEL = "Label", U_FOLDOUT = "Foldout";
        
        private readonly Type _spriteType = typeof(Sprite);
        private readonly string[][] _ability = { HumanAbilityId.Names_Ed, ActorAbilityId.Names_Ed };
                
        protected VisualElement CreateGUI<TId>(string captionText) where TId : APerkId<TId>
        {
            int typePerks = typeof(TId) == typeof(EconomicPerksId) ? AbilityTypeId.Economic : AbilityTypeId.Military;

            var root = _treePerksVT.CloneTree();
            root.Q<Label>(U_LABEL).text = captionText;
            var container = root.Q<VisualElement>(U_CONTAINER);

            SerializedProperty propertyPerks = serializedObject.FindProperty(P_PERKS).GetArrayElementAtIndex(typePerks).FindPropertyRelative(P_PERK_VALUES);
            propertyPerks.arraySize = APerkId<TId>.Count;
            serializedObject.ApplyModifiedProperties();
                        
            for (int i = 0; i < APerkId<TId>.Count; i++)
            {
                int id = i;
                SerializedProperty propertyPerk = propertyPerks.GetArrayElementAtIndex(i);
                string name = APerkId<TId>.DisplayNames_Ed[i];

                VisualElement element = _treePerkVT.Instantiate(propertyPerk.propertyPath);
                element.Q<Label>(U_LABEL).text = name;
                var foldout = element.Q<Foldout>(U_FOLDOUT);
                foldout.value = propertyPerk.isExpanded;
                foldout.RegisterValueChangedCallback(evt => propertyPerk.isExpanded = evt.newValue);
                element.Q<IMGUIContainer>(U_CONTAINER).onGUIHandler = () => IMGUIPerk<TId>(typePerks, id, propertyPerk, name);

                container.Add(element);
            }

            return root;
        }

        private void IMGUIPerk<TId>(int typePerks, int id, SerializedProperty propertyPerk, string name) where TId : APerkId<TId>
        {
            int target = -1, ability = -1;
            string abilityName = name.Split('_')[0];
            for (int i = 0; i < TargetOfPerkId.Count; i++)
            {
               if((ability = Array.IndexOf(_ability[i], abilityName)) >= 0)
               {
                    target = i; break;
               }
            }

            if (target < 0 || ability < 0)
            {
                HelpBox($"Wrong name {name}", UnityEditor.MessageType.Error);
                return;
            }

            //serializedObject.Update();

            propertyPerk.FindPropertyRelative(P_ID).intValue = id;
            propertyPerk.FindPropertyRelative(P_TYPE).intValue = typePerks;

            DrawEndSet(P_TARGET_OBJ, target, TargetOfPerkId.Names_Ed[target]);
            DrawEndSet(P_TARGET_AB, ability, _ability[target][ability]);

            int mod = DrawModifier();
            DrawValue(mod, target, typePerks, ability);
            Space();

            DrawLevel();
            DrawPosition();
            Space();

            DrawDesc(abilityName);
            DrawSprite();
            Space();

            serializedObject.ApplyModifiedProperties();

            #region Local: DrawEndSet(..), DrawLevel(), DrawPosition(..), DrawModifier(), DrawValue(..), DrawDesc(..), DrawSprite()
            //================================================================
            void DrawEndSet(string nameProperty, int value, string text)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(nameProperty);
                property.intValue = value;
                LabelField(property.displayName, text);
            }
            //================================================================
            void DrawLevel()
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_LEVEL);
                int value = Mathf.Clamp(property.intValue, MIN_LEVEL, MAX_LEVEL);
                property.intValue = value = IntSlider(property.displayName, value, MIN_LEVEL, MAX_LEVEL);

                propertyPerk.FindPropertyRelative(P_COST).intValue = value + 1;
            }
            //================================================================
            void DrawPosition()
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_POS);
                int value = Mathf.Clamp(property.intValue, MIN_POS, MAX_POS);
                value = IntSlider(property.displayName, value, MIN_POS, MAX_POS);
                property.intValue = value;            
            }
            //================================================================
            int DrawModifier()
            {
                SerializedProperty modProperty = propertyPerk.FindPropertyRelative(P_PERK_MOD);
                int value = modProperty.intValue = IntPopup(modProperty.displayName, modProperty.intValue, PerkModifierId.Names_Ed, TypeModifierId.Values_Ed);
                propertyPerk.FindPropertyRelative(P_TYPE_OP).intValue = PerkModifierId.ToTypeModifier(value);
                return value;
            }
            //================================================================
            void DrawValue(int mod, int target, int type, int ability)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_VALUE);
                string name = "Value";

                if (mod == PerkModifierId.Enable)
                {
                    property.intValue = 1;
                    BeginDisabledGroup(true);
                        Toggle(name, true);
                    EndDisabledGroup();
                    return;
                }

                int min = -1, max = 10, shift = 0;
                if (mod == PerkModifierId.Percent || (type == EconomicPerksId.Type & ability == HumanAbilityId.ExchangeSaleChance))
                {
                    name = "Value (%)";
                    min = 0; max = 50;
                }
                else if (target == TargetOfPerkId.Warriors & ability <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                {
                    shift = ActorAbilityId.SHIFT_ABILITY;
                }

                property.intValue = IntSlider(name, property.intValue >> shift, min, max) << shift;
            }
            //================================================================
            void DrawDesc(string name)
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_KEY_DESC);
                property.stringValue = PREFF_KEY_DESC.Concat(name);
                TextField(property.displayName, property.stringValue);
            }
            //================================================================
            void DrawSprite()
            {
                SerializedProperty property = propertyPerk.FindPropertyRelative(P_SPRITE);
                property.objectReferenceValue = ObjectField(property.displayName, property.objectReferenceValue, _spriteType, false);
            }
            #endregion
        }

    }
}
