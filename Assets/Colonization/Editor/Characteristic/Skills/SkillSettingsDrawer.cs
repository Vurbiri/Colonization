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
        private const string P_RANGE = "range", P_TARGET = "target", P_COST = "cost", P_HITS = "effectsHits", P_SFX = "SFXHits", P_EFFECTS = "_effects";
        private const string P_SPRITE = "_sprite", P_KEY_NAME = "_nameKey", P_COST_UI = "_cost";
        private const string P_CHILD_TARGET = "_parentTarget";

        private readonly string[] KEYS_NAME_SKILLS = { "Attack", "MagicAttack", "Sweep", "Combo", "Heal" };

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
                var clip = DrawChildrenObject<AnimationClipSettingsScriptable>(P_CLIP);

                if (clip != null && clip.clip != null)
                {
                    DrawButton(clip);

                    SerializedProperty costProperty = mainProperty.FindPropertyRelative(P_COST);
                    SerializedProperty uiProperty = mainProperty.FindPropertyRelative(P_UI);

                    DrawLine(Color.gray);
                    EditorGUI.indentLevel++;

                    DrawLabel("Total Time", $"{clip.totalTime} c");
                    DrawLabel("Hit Time", $"{string.Join("% ", clip.hitTimes)}%");
                    DrawLabel("Remaining Time", $"{clip.totalTime * (100f - clip.hitTimes[^1])/100f} c");
                    DrawLabelAndSetValue(P_RANGE, clip.range);
                    EditorGUI.indentLevel--;
                    DrawLine(Color.gray);
                    Space();

                    TargetOfSkill target = DrawEnumPopup<TargetOfSkill>(P_TARGET);

                    Space();
                    DrawBool(P_MOVE);
                    DrawBool(P_REACT);

                    Space();
                    DrawSelfIntSlider(costProperty, 0, 3);
                                        
                    uiProperty.FindPropertyRelative(P_COST_UI).intValue = costProperty.intValue;

                    Space(2f);
                    DrawStringPopup(uiProperty, P_KEY_NAME, KEYS_NAME_SKILLS);
                    DrawChildrenObject<Sprite>(uiProperty, P_SPRITE, true);

                    DrawLine(Color.gray);
                    DrawHits(clip.hitTimes.Length, target);
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
            //=================================
            void DrawHits(int count, TargetOfSkill target)
            {
                if (count <= 0) return;

                SerializedProperty SFXsProperty = _mainProperty.FindPropertyRelative(P_SFX);
                SerializedProperty hitsProperty = _mainProperty.FindPropertyRelative(P_HITS);

                TrySetArraySize(SFXsProperty, count);
                TrySetArraySize(hitsProperty, count);
                
                SerializedProperty effectsProperty, effectProperty;
                for (int i = 0; i < count; i++)
                {
                    
                    effectsProperty = hitsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(P_EFFECTS);
                    if (effectsProperty.arraySize == 0)
                        effectsProperty.InsertArrayElementAtIndex(0);

                    EditorGUI.indentLevel--;
                    DrawObject<AHitScriptableSFX>(SFXsProperty.GetArrayElementAtIndex(i), $"SFX Hit {i}");
                    EditorGUI.indentLevel++;
                    _position.y += _height;
                    EditorGUI.PropertyField(_position, effectsProperty, new GUIContent($"Hit {i}"));
                    
                    for (int j = 0; j < effectsProperty.arraySize; j++)
                    {
                        effectProperty = effectsProperty.GetArrayElementAtIndex(j);
                        effectProperty.FindPropertyRelative(P_CHILD_TARGET).SetEnumValue(target);
                        if (effectsProperty.isExpanded)
                            _position.y += _height * EffectSettingsDrawer.GetPropertyRateHeight(effectsProperty.GetArrayElementAtIndex(j));
                    }
                    if (effectsProperty.isExpanded)
                        _position.y += _height * 1.8f; // "+ -" панель
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
                    rate += 14.1f;

                    SerializedProperty hitsProperty = property.FindPropertyRelative(P_HITS);
                    SerializedProperty effectsProperty;

                    for (int i = 0; i < hitsProperty.arraySize; i++)
                    {
                        rate += 2f;
                        effectsProperty = hitsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(P_EFFECTS);
                        if (effectsProperty.isExpanded)
                        {
                            rate += 1.8f;
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
