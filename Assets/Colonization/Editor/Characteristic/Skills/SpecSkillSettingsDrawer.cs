using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;
using static Vurbiri.Colonization.ASkillSettings;
using static Vurbiri.Colonization.SpecSkillSettings;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(SpecSkillSettings))]
    sealed public class SpecSkillSettingsDrawer : ASkillSettingsDrawer
    {
        protected override void OnGUI()
		{

            if (GetProperty(P_TYPE).intValue == ActorTypeId.Warrior)
            {
                DrawHeading("Block");
                DrawBlock();
            }
            else
            {
                DrawHeading("Spec Skill");
                if (_mainProperty.isExpanded)
                {
                    _position.y += _ySpace;
                    int hitsCount = 1;
                    if (!nonClip.Contains(GetProperty(actorIdField).intValue))
                        hitsCount = DrawClip(GetObject<AnimationClipSettingsScriptable>(P_CLIP), 15f, true);
                    DrawMain(hitsCount);
                }
            }

            #region Local: DrawHeading(..), DrawBlock(), DrawMain(..), DrawButton(..)
            //=================================
            void DrawHeading(string caption)
            {
                const float offset = 4f;
                
                _position.height += offset;
                LabelField(_position, caption, STYLES.H3);
                _position.height -= offset;
                _position.y += offset;

                DrawButton(offset * 0.5f);
            }
            //=================================
            void DrawBlock()
            {
                if (_mainProperty.isExpanded)
                {
                    var effectProperty = GetProperty(valueField);

                    _position.y += _ySpace;
                    BeginProperty();
                    {
                        DrawInt(costField, 1, 4, 1);
                        effectProperty.intValue = DrawInt(effectProperty, 5, 60, effectProperty.intValue >> ActorAbilityId.SHIFT_ABILITY) << ActorAbilityId.SHIFT_ABILITY;
                    }
                    EndProperty();
                }
            }
            //=================================
            void DrawMain(int hitsCount)
            {
                if (hitsCount > 0)
                {
                    BeginProperty();
                    {
                        TargetOfSkill target = DrawEnum<TargetOfSkill>(targetField);

                        Space();
                        DrawInt(costField, 0, 4, 1);
                        DrawInt(valueField, "Adv Value", -100, 100, 0);

                        DrawLine(15f);
                        DrawProperty(hitSFXField, "SFX Name");
                        indentLevel++;
                        DrawHits(hitsCount, target);
                        indentLevel--;
                    }
                    EndProperty();
                }
            }
            //=================================
            void DrawButton(float offset)
            {
                Rect positionButton = _position;
                const float size = 49f;

                positionButton.y -= offset;
                positionButton.x = (positionButton.width - size) + 17f;
                positionButton.width = size;

                if (GUI.Button(positionButton, _mainProperty.isExpanded ? "Hide" : "Show"))
                    _mainProperty.isExpanded = !_mainProperty.isExpanded;
            }
            //=================================
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			float rate = 1.1f;
            if (mainProperty.isExpanded)
            {
                if (mainProperty.FindPropertyRelative(P_TYPE).intValue == ActorTypeId.Warrior)
                    rate = 3.2f;
                else
                    rate = GetPropertyRate(mainProperty, !nonClip.Contains(mainProperty.FindPropertyRelative(actorIdField).intValue), -2.4f);
            }

            return _height * rate;
		}


        
    }
}