using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static UnityEditor.EditorGUI;
using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

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
        private const string P_CLIP = "clipSettings_ed", P_SFX = "hitSFXName_ed", P_TYPE = "typeActor_ed";
        private const string P_CHILD_TARGET = "_parentTarget_ed", P_CHILD_TYPE = "_isWarrior_ed";

        private static readonly string[] KEYS_NAME_SKILLS =
        { "Strike", "Swipe", "Combo", "Heal", "Sparks", "Toxin", "Bolt", "Swarm", "Battlecry", "Fortify", "WeaponEnhancement", "ArmorEnhancement", "Kick", "Leap" };
        #endregion

        protected override void OnGUI()
        {
            SerializedProperty uiProperty = GetProperty(P_UI);
            SerializedProperty keyNameProperty = GetProperty(uiProperty, P_KEY_NAME);

            SetName(keyNameProperty);

            BeginProperty();
            indentLevel++;

            if (Foldout())
            {
                indentLevel++;

                Space();
                var clip = DrawObject<AnimationClipSettingsScriptable>(P_CLIP, false);

                if (clip != null && clip.clip != null)
                {
                    SerializedProperty costProperty = GetProperty(P_COST);

                    DrawButton(clip);

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


                    GetProperty(uiProperty, P_COST_UI).intValue = costProperty.intValue;

                    Space(2f);
                    DrawLabel("UI:");
                    indentLevel++;
                    DrawStringPopup(keyNameProperty, KEYS_NAME_SKILLS);
                    DrawObjectRelative<Sprite>(uiProperty, P_SPRITE, true);
                    indentLevel--;

                    DrawHits(clip.hitTimes.Length, target);
                }

                indentLevel--;
            }

            indentLevel--;
            EndProperty();

            #region Local: SetName(..), DrawButton(..), DrawHits(..)
            //=================================
            void SetName(SerializedProperty property)
            {
                string name;
                if (string.IsNullOrEmpty(property.stringValue))
                    name = "None";
                else
                    name = Localization.ForEditor(FILE).GetText(FILE, property.stringValue).Delete("<b>", "</b>");

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
                    Selection.activeObject = activeObject;

                _position.y += _ySpace * 2f;
            }
            //=================================
            void DrawHits(int count, TargetOfSkill target)
            {
                if (count <= 0) return;

                SerializedProperty SFXProperty = GetProperty(P_SFX);
                SerializedProperty hitsProperty = GetProperty(P_HITS);

                bool isWarrior = GetProperty(P_TYPE).intValue == ActorTypeId.Warrior;

                if (hitsProperty.arraySize != count)
                    hitsProperty.arraySize = count;

                DrawLine(40f); indentLevel--;
                _position.y += _height;
                
                PropertyField(_position, SFXProperty);
                _position.y += _ySpace;

                SerializedProperty effectsProperty, effectProperty;
                for (int i = 0; i < count; i++)
                {
                    effectsProperty = GetProperty(hitsProperty.GetArrayElementAtIndex(i), P_EFFECTS);
                    if (effectsProperty.arraySize == 0)
                        effectsProperty.InsertArrayElementAtIndex(0);

                    _position.y += _height;
                    PropertyField(_position, effectsProperty, new GUIContent($"Hit {i}"));
                    
                    for (int j = 0; j < effectsProperty.arraySize; j++)
                    {
                        effectProperty = effectsProperty.GetArrayElementAtIndex(j);
                        SetEnum(effectProperty, P_CHILD_TARGET, target);
                        SetBool(effectProperty, P_CHILD_TYPE, isWarrior);

                        if (effectsProperty.isExpanded)
                            _position.y += _height * HitEffectSettingsDrawer.GetPropertyRateHeight(effectProperty, j);
                    }

                    if (effectsProperty.isExpanded)
                        _position.y += _height * 1.8f;
                }
                indentLevel++;
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
                    rate += 14.2f;

                    SerializedProperty hitsProperty = property.FindPropertyRelative(P_HITS);
                    SerializedProperty effectsProperty;

                    for (int i = 0; i < hitsProperty.arraySize; i++)
                    {
                        rate += 1.1f;
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
