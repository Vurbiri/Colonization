using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization.Characteristics
{
	public abstract class ASkillSettingsDrawer : PropertyDrawerUtility
    {
        private const string P_RANGE = "_range", P_DISTANCE = "_distance";
        private const string P_CHILD_TARGET = "_parentTarget_ed", P_CHILD_TYPE = "_isWarrior_ed";

        protected const string P_TARGET = "_target", P_COST = "_cost";
        protected const string P_HITS = "_effectsHitsSettings";
        protected const string P_EFFECTS = "_effects";
        protected const string P_SPRITE_UI = "sprite", P_KEY_NAME_UI = "keySkillName";
        protected const string P_CLIP = "clipSettings_ed", P_TYPE = "typeActor_ed";

        protected int DrawClip(AnimationClipSettingsScriptable clip, float offsetLine = 40f)
        {
            Space();

            int hitsCount = 0;

            if (clip != null && clip.clip != null)
            {
                hitsCount = clip.hitTimes.Length;

                DrawButton(clip, offsetLine);

                DrawLine(offsetLine);
                indentLevel++;

                DrawLabel("Total Time", $"{clip.totalTime} c");
                DrawLabel("Hit Time", $"{string.Join("% ", clip.hitTimes)}%");
                DrawLabel("Remaining Time", $"{clip.totalTime * (100f - clip.hitTimes[^1]) / 100f} c");
                SetLabelFloat(P_RANGE, clip.range);
                SetLabelFloat(P_DISTANCE, clip.distance);
                indentLevel--;
                DrawLine(offsetLine);
                Space();
            }

            return hitsCount;

            #region Local: DrawButton(..)
            //=================================
            void DrawButton(AnimationClipSettingsScriptable activeObject, float offset)
            {
                _position.y += _height;
                Rect positionButton = _position;
                const float size = 350f;// EditorGUIUtility.currentViewWidth;

                positionButton.height += _ySpace * 2f;
                positionButton.x = (positionButton.width - size) * 0.5f + offset;
                positionButton.width = size;


                if (GUI.Button(positionButton, "Select Clip Settings".ToUpper()))
                    Selection.activeObject = activeObject;

                _position.y += _ySpace * 2f;
            }
            //=================================
            #endregion
        }

        protected void DrawHits(int count, TargetOfSkill target)
        {
            if (count <= 0) return;

            SerializedProperty hitsProperty = GetProperty(P_HITS);

            bool isWarrior = GetProperty(P_TYPE).intValue == ActorTypeId.Warrior;

            if (hitsProperty.arraySize != count)
                hitsProperty.arraySize = count;

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
        }

        protected static float GetPropertyRate(SerializedProperty property, bool useClip = true, float offset = 0f)
        {
            float rate = 1f;

            if (property.isExpanded)
            {
                rate = 2.5f;
                AnimationClipSettingsScriptable clipSett = property.FindPropertyRelative(P_CLIP).objectReferenceValue as AnimationClipSettingsScriptable;
                bool isClip = clipSett != null && clipSett.clip != null;
                if (isClip | !useClip)
                {
                    rate += (isClip ? 14.2f : 6.7f) + offset;

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

            return rate;
        }
    }
}