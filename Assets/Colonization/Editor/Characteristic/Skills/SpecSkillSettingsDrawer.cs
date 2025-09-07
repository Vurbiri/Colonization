using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization.Characteristics
{
	[CustomPropertyDrawer(typeof(SpecSkillSettings))]
    sealed public class SpecSkillSettingsDrawer : ASkillSettingsDrawer
    {
        private const string P_SFX = "_hitSFXName", P_VALUE = "_value";
        private const string P_ID = "_actorId_Ed";

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
                    if (!SpecSkillSettings.nonClip.Contains(GetProperty(P_ID).intValue))
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
                    var effectProperty = GetProperty(P_VALUE);

                    _position.y += _ySpace;
                    BeginProperty();
                    {
                        DrawInt(P_COST, 1, 4, 1);
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
                        TargetOfSkill target = DrawEnum<TargetOfSkill>(P_TARGET);

                        Space();
                        DrawInt(P_COST, 0, 4, 1);
                        DrawInt(P_VALUE, "Adv Value", -60, 60, 0);

                        DrawLine(15f);
                        DrawProperty(P_SFX, "SFX Name");
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
                    rate = GetPropertyRate(mainProperty, !SpecSkillSettings.nonClip.Contains(mainProperty.FindPropertyRelative(P_ID).intValue), -2.27f);
            }

            return _height * rate;
		}


        
    }
}