//Assets\Colonization\Editor\Characteristic\Skills\SkillSettingsDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomPropertyDrawer(typeof(SkillSettings))]
    public class SkillSettingsDrawer : PropertyDrawerUtility
    {
        private const string NAME_ELEMENT = "Skill {0}";
        private const string P_RANGE = "_range", P_DISTANCE = "_distance", P_TARGET = "_target", P_COST = "_cost";
        private const string P_HITS = "_effectsHitsSettings", P_UI = "_ui";
        private const string P_CLIP = "clipSettings", P_SFX = "SFXHits";
        private const string P_EFFECTS = "_effects";
        private const string P_SPRITE = "_sprite", P_KEY_NAME = "_idNameKey", P_COST_UI = "_cost";
        private const string P_CHILD_TARGET = "_parentTarget";

        public override void OnGUI(Rect mainPosition, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(mainPosition, mainProperty, label);

            int id = IdFromLabel(label);
            if (id >= 0)
                label.text = string.Format(NAME_ELEMENT, id);

            label = EditorGUI.BeginProperty(_position, label, mainProperty);
            Level++;

            if (Foldout(label))
            {
                Level++;

                Space();
                var clip = DrawObject<AnimationClipSettingsScriptable>(P_CLIP, false);

                if (clip != null && clip.clip != null)
                {
                    DrawButton(clip);

                    SerializedProperty costProperty = GetProperty(P_COST);
                    SerializedProperty uiProperty = GetProperty(P_UI);

                    DrawLine(40f);
                    Level++;

                    DrawLabel("Total Time", $"{clip.totalTime} c");
                    DrawLabel("Hit Time", $"{string.Join("% ", clip.hitTimes)}%");
                    DrawLabel("Remaining Time", $"{clip.totalTime * (100f - clip.hitTimes[^1])/100f} c");
                    SetLabelFloat(P_RANGE, clip.range);
                    SetLabelFloat(P_DISTANCE, clip.distance);
                    Level--;
                    DrawLine(40f);
                    Space();

                    TargetOfSkill target = DrawEnum<TargetOfSkill>(P_TARGET);

                    Space();
                    DrawInt(costProperty, 1, 3, 1);
                                        
                    GetProperty(uiProperty, P_COST_UI).intValue = costProperty.intValue;

                    Space(2f);
                    DrawLabel("UI:");
                    Level++;
                    DrawIntPopupRelative(uiProperty, P_KEY_NAME, KEYS_NAME_SKILLS);
                    DrawObjectRelative<Sprite>(uiProperty, P_SPRITE, true);
                    Level--;

                    DrawLine(40f);
                    DrawHits(clip.hitTimes.Length, target);
                 }

                Level--;
            }

            Level--;
            EditorGUI.EndProperty();

            #region Local: DrawButton(...)
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

                SerializedProperty SFXsProperty = GetProperty(P_SFX);
                SerializedProperty hitsProperty = GetProperty(P_HITS);

                SFXsProperty.arraySize = count;
                hitsProperty.arraySize = count;
                
                SerializedProperty effectsProperty, effectProperty;
                for (int i = 0; i < count; i++)
                {
                    effectsProperty = GetProperty(hitsProperty.GetArrayElementAtIndex(i), P_EFFECTS);
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
                        GetProperty(effectProperty, P_CHILD_TARGET).SetEnum(target);
                        if (effectsProperty.isExpanded)
                            _position.y += _height * HitEffectSettingsDrawer.GetPropertyRateHeight(effectsProperty.GetArrayElementAtIndex(j), j);
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
                    rate += 13.1f;

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
                                rate += HitEffectSettingsDrawer.GetPropertyRateHeight(effectsProperty.GetArrayElementAtIndex(j), j);
                        }
                    }
                }
            }

            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * rate;
        }
    }
}
