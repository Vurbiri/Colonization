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
                Rect position = GetPosition(offsetLine);

                DrawButton(ref position, clip);

                position.height = _position.height;

                DrawLabel(ref position, "Total Time", $"{clip.totalTime} c");
                DrawLabel(ref position, "Hit Time", $"{string.Join("% ", clip.hitTimes)}%");
                DrawLabel(ref position, "Remaining Time", $"{clip.totalTime * (100f - clip.hitTimes[^1]) / 100f} c");

                _position.y = position.y;

                DrawLine(offsetLine);

                DrawSlider(P_RANGE, 10f);
                DrawSlider(P_DISTANCE, 20f);

                DrawLine(offsetLine);
                Space();

                hitsCount = clip.hitTimes.Length;
            }

            return hitsCount;

            #region Local: GetPosition(..), DrawButton(..), DrawLabel(..), DrawSlider(..)
            //=================================
            Rect GetPosition(float offset)
            {
                const float size = 350f;
                Rect position = _position;

                position.height += _ySpace * 2f;
                position.x = (position.width - size) * 0.5f + offset;
                position.width = size;

                return position;
            }
            //=================================
            void DrawButton(ref Rect position, AnimationClipSettingsScriptable activeObject)
            {
                position.y += _height;

                if (GUI.Button(position, "Select Clip Settings".ToUpper()))
                    Selection.activeObject = activeObject;

                position.y += _ySpace * 3f;
            }
            //=================================
            void DrawLabel(ref Rect position, string displayName, string value)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                LabelField(position, displayName, value);
            }
            //=================================
            void DrawSlider(string name, float max)
            {
                var property = GetProperty(name);
                if (Mathf.Approximately(property.floatValue, 0f))
                    property.floatValue = -1;
                DrawFloat(property, -1f, max, -1f);
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
                    rate += (isClip ? 14f : 6.7f) + offset;

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