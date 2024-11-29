//Assets\Colonization\Editor\Characteristic\Skills\SkillSettingsDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomPropertyDrawer(typeof(Skills.SkillSettings))]
    public class SkillSettingsDrawer : PropertyDrawerUtility
    {
        private const string NAME_ELEMENT = "Skill {0}";
        private const string P_CLIP = "clipSettings", P_MOVE = "isMove", P_REACT = "isTargetReact", P_UI = "ui";
        private const string P_RANGE = "range", P_TARGET = "target", P_COST = "cost", P_PACKETS = "effectsPacket", P_EFFECTS = "_effects";
        private const string P_SPRITE = "_sprite", P_KEY_NAME = "_nameKey", P_COST_UI = "_cost";
        private const string P_CHILD_TARGET = "_parentTarget";
        private readonly string[] KEYS_NAME_SKILLS = { "Attack", "Sweep", "Combo" };

        public override void OnGUI(Rect mainPosition, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(mainPosition, mainProperty, label);

            int id = IdFromLabel(label);
            if (id >= 0)
                label.text = string.Format(NAME_ELEMENT, id);

            label = EditorGUI.BeginProperty(_position, label, mainProperty);
            EditorGUI.indentLevel++;

            if (Foldout(label))
            {
                EditorGUI.indentLevel++;

                Space();
                var clip = DrawObject<AnimationClipSettingsScriptable>(P_CLIP);

                if (clip != null && clip.clip != null)
                {
                    DrawButton(clip);

                    SerializedProperty costProperty = mainProperty.FindPropertyRelative(P_COST);
                    SerializedProperty uiProperty = mainProperty.FindPropertyRelative(P_UI);

                    DrawLine(Color.gray);
                    EditorGUI.indentLevel++;

                    DrawLabel("Total Time", $"{clip.totalTime}");
                    DrawLabel("Damage Time", string.Join(" ", clip.damageTimes));
                    DrawLabel("Remaining Time", $"{clip.RemainingTime}");
                    DrawLabelAndSetValue(P_RANGE, clip.range);
                    EditorGUI.indentLevel--;
                    DrawLine(Color.gray);

                    Space();

                    TargetOfSkill target;
                    if ((target = DrawEnumPopup<TargetOfSkill>(P_TARGET)) == TargetOfSkill.Enemy)
                    {
                        Space();
                        if (DrawBool(P_MOVE))
                            DrawLabelAndSetValue(P_REACT, true);
                        else
                            DrawBool(P_REACT);
                    }
                    else
                    {
                        DrawLabelAndSetValue(P_MOVE, false);
                        DrawLabelAndSetValue(P_REACT, false);
                    }

                    Space();
                    DrawSelfIntSlider(costProperty, 0, 3);
                                        
                    uiProperty.FindPropertyRelative(P_COST_UI).intValue = costProperty.intValue;

                    Space(2f);
                    DrawStringPopup(uiProperty, P_KEY_NAME, KEYS_NAME_SKILLS);
                    DrawObject<Sprite>(uiProperty, P_SPRITE, true);

                    Space(2f);
                    DrawPackets(clip.damageTimes.Length, target);
                   
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();

            #region Local: DrawSelfIntSlider(..), DrawButton(...)
            //=================================
            void DrawSelfIntSlider(SerializedProperty property, int min, int max)
            {
                _position.y += _height;
                property.intValue = EditorGUI.IntSlider(_position, property.displayName, property.intValue, min, max);
            }
            //=================================
            void DrawButton(UnityEngine.Object activeObject)
            {
                _position.y += _height;
                Rect positionButton = _position;
                float viewWidth = EditorGUIUtility.currentViewWidth;

                positionButton.height += _ySpace * 2f;
                positionButton.x = 100f;
                positionButton.width = viewWidth - 125f;

                if (GUI.Button(positionButton, "Select Clip Settings".ToUpper()))
                {
                    //AnimationClipSettingsWindow.ShowWindow();
                    Selection.activeObject = activeObject;
                }

                _position.y += _ySpace * 2f;
            }
            void DrawPackets(int count, TargetOfSkill target)
            {
                SerializedProperty packetsProperty = _mainProperty.FindPropertyRelative(P_PACKETS);
                while (packetsProperty.arraySize > count)
                    packetsProperty.DeleteArrayElementAtIndex(packetsProperty.arraySize - 1);
                while (packetsProperty.arraySize < count)
                    packetsProperty.InsertArrayElementAtIndex(packetsProperty.arraySize);

                SerializedProperty effectsProperty, effectProperty;
                for (int i = 0; i < count; i++)
                {
                    effectsProperty = packetsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(P_EFFECTS);
                    if (effectsProperty.arraySize == 0)
                        effectsProperty.InsertArrayElementAtIndex(0);

                    _position.y += _height;
                    EditorGUI.PropertyField(_position, effectsProperty, new GUIContent($"Hint {i}"));
                    if (effectsProperty.isExpanded)
                    { 
                        for (int j = 0; j < effectsProperty.arraySize; j++)
                        {
                            effectProperty = effectsProperty.GetArrayElementAtIndex(j);
                            effectProperty.FindPropertyRelative(P_CHILD_TARGET).SetEnumValue(target);
                            _position.y += _height * EffectSettingsDrawer.GetPropertyRateHeight(effectsProperty.GetArrayElementAtIndex(j));
                        }
                        _position.y += _height * 2f;
                    }
                }
                
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1f;

            if (property.isExpanded)
            {
                rate *= 2.5f;
                AnimationClipSettingsScriptable clipSett = property.FindPropertyRelative(P_CLIP).objectReferenceValue as AnimationClipSettingsScriptable;
                if (clipSett != null && clipSett.clip != null)
                {
                    rate += 13.1f;

                    SerializedProperty packetsProperty = property.FindPropertyRelative(P_PACKETS);
                    SerializedProperty effectsProperty;

                    for (int i = 0; i < packetsProperty.arraySize; i++)
                    {
                        rate += 1f;
                        effectsProperty = packetsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(P_EFFECTS);
                        if (effectsProperty.isExpanded)
                        {
                            rate += 2.1f;
                            for (int j = 0; j < effectsProperty.arraySize; j++)
                                rate += EffectSettingsDrawer.GetPropertyRateHeight(effectsProperty.GetArrayElementAtIndex(j));
                        }
                    }
                }
            }

            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * rate;
        }
    }
}
