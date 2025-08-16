using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomPropertyDrawer(typeof(SkillSettings))]
    public class SkillSettingsDrawer : PropertyDrawerUtility
    {
        #region Consts
        private const string P_RANGE = "_range", P_DISTANCE = "_distance", P_TARGET = "_target", P_COST = "_cost";
        private const string P_HITS = "_effectsHitsSettings", P_UI = "_ui";
        private const string P_EFFECTS = "_effects";
        private const string P_SPRITE = "_sprite", P_KEY_NAME = "_keyName", P_COST_UI = "_cost";
        private const string P_CHILD_TARGET = "_parentTarget_ed", P_CLIP = "clipSettings_ed", P_SFX = "hitSFXs", P_TYPE = "typeActor_ed";

        private static readonly string[] KEYS_NAME_SKILLS =
        { "Strike", "Channel", "Sweep", "Combo", "Heal", "Sparks", "Battlecry", "Fortify", "Enhancement", "Kick", "Leap" };
        #endregion

        private readonly Color _positive = new(0.5f, 1f, 0.3f, 1f), _negative = new(1f, 0.5f, 0.3f, 1f);

        protected override void OnGUI()
        {
            bool isWarrior = GetProperty(P_TYPE).intValue == ActorTypeId.Warrior;

            SerializedProperty uiProperty = GetProperty(P_UI);
            SerializedProperty keyNameProperty = GetProperty(uiProperty, P_KEY_NAME);

            SetName(keyNameProperty, isWarrior);

            BeginProperty();
            indentLevel++;

            if (Foldout())
            {
                indentLevel++;

                Space();
                var clip = DrawObject<AnimationClipSettingsScriptable>(P_CLIP, false);

                if (clip != null && clip.clip != null)
                {
                    DrawButton(clip);

                    SerializedProperty costProperty = GetProperty(P_COST);

                    DrawLine(40f);
                    indentLevel++;

                    DrawLabel("Total Time", $"{clip.totalTime} c");
                    DrawLabel("Hit Time", $"{string.Join("% ", clip.hitTimes)}%");
                    DrawLabel("Remaining Time", $"{clip.totalTime * (100f - clip.hitTimes[^1])/100f} c");
                    SetLabelFloat(P_RANGE, clip.range);
                    SetLabelFloat(P_DISTANCE, clip.distance);
                    indentLevel--;
                    DrawLine(40f);
                    Space();

                    TargetOfSkill target = DrawEnum<TargetOfSkill>(P_TARGET);

                    Space();
                    DrawInt(costProperty, 1, 4, 1);

                    if (isWarrior)
                    {
                        GetProperty(uiProperty, P_COST_UI).intValue = costProperty.intValue;

                        Space(2f);
                        DrawLabel("UI:");
                        indentLevel++;
                        DrawStringPopup(keyNameProperty, KEYS_NAME_SKILLS);
                        DrawObjectRelative<Sprite>(uiProperty, P_SPRITE, true);
                        indentLevel--;
                    }
                    else
                    {
                        keyNameProperty.stringValue = null;
                        uiProperty.FindPropertyRelative(P_SPRITE).objectReferenceValue = null;
                    }

                    DrawHits(clip.hitTimes.Length, target);
                 }

                indentLevel--;
            }

            indentLevel--;
            EndProperty();

            #region Local: SetName(..), DrawButton(..), DrawHits(..)
            //=================================
            void SetName(SerializedProperty property, bool isWarrior)
            {
                string name = isWarrior ? Localization.ForEditor(LangFiles.Actors).GetText(property.stringValue).Delete("<b>", "</b>") : "Skill";
                int id = IdFromLabel();
                if (id >= 0) name = string.Concat($"[{id}] ", name);
                _label.text = name;
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

                SerializedProperty SFXsProperty = GetProperty(P_SFX);
                SerializedProperty hitsProperty = GetProperty(P_HITS);

                if (SFXsProperty.arraySize != count) SFXsProperty.arraySize = count;
                if (hitsProperty.arraySize != count) hitsProperty.arraySize = count;

                Color lineColor;
                
                SerializedProperty effectsProperty, effectProperty;
                for (int i = 0; i < count; i++)
                {
                    effectsProperty = GetProperty(hitsProperty.GetArrayElementAtIndex(i), P_EFFECTS);
                    if (effectsProperty.arraySize == 0)
                        effectsProperty.InsertArrayElementAtIndex(0);

                    lineColor = target switch
                    {
                        TargetOfSkill.Enemy  => _negative,
                        TargetOfSkill.Friend => _positive,
                        TargetOfSkill.Self   => Color.magenta,
                        _                    => Color.gray,
                    };

                    DrawLine(lineColor, 40f);

                    indentLevel--;
                    _position.y += _height;
                    PropertyField(_position, SFXsProperty.GetArrayElementAtIndex(i), new GUIContent($"SFX Hit {i}"));
                    indentLevel++;
                    _position.y += _height;
                    PropertyField(_position, effectsProperty, new GUIContent($"Hit {i}"));
                    
                    for (int j = 0; j < effectsProperty.arraySize; j++)
                    {
                        effectProperty = effectsProperty.GetArrayElementAtIndex(j);
                        GetProperty(effectProperty, P_CHILD_TARGET).SetEnum(target);

                        if (effectsProperty.isExpanded)
                            _position.y += _height * HitEffectSettingsDrawer.GetPropertyRateHeight(effectProperty, j);
                    }

                    if (effectsProperty.isExpanded)
                        _position.y += _height * 1.8f;
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
                    bool isWarrior = property.FindPropertyRelative(P_TYPE).intValue == ActorTypeId.Warrior;

                    if (isWarrior) rate += 13.1f; else rate += 9.8f;

                    SerializedProperty hitsProperty = property.FindPropertyRelative(P_HITS);
                    SerializedProperty effectsProperty;

                    for (int i = 0; i < hitsProperty.arraySize; i++)
                    {
                        rate += 2.3f;
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
