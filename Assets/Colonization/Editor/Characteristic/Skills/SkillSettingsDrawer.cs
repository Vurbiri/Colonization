using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static UnityEditor.EditorGUI;
using static Vurbiri.Colonization.UI.CONST_UI;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomPropertyDrawer(typeof(SkillSettings), false)]
    sealed public class SkillSettingsDrawer : ASkillSettingsDrawer
    {
        private const string P_UI = "_ui";
        private const string P_SFX = "hitSFXName_ed";

        private static readonly string[] KEYS_NAME_SKILLS =
        { "Strike", "Swipe", "Combo", "Heal", "Sparks", "Toxin", "Bolt", "Swarm", "Battlecry", "Fortify", "WeaponEnhancement", "ArmorEnhancement", "Kick", "Leap" };

        protected override void OnGUI()
        {
            SerializedProperty uiProperty = GetProperty(P_UI);
            SerializedProperty keyNameProperty = GetProperty(uiProperty, P_KEY_NAME_UI);

            SetName(keyNameProperty);

            BeginProperty();
            indentLevel++;

            if (Foldout())
            {
                indentLevel++;
                var clip = DrawObject<AnimationClipSettingsScriptable>(P_CLIP, false);
                int hitsCount = DrawClip(clip);

                if (hitsCount > 0)
                {
                    TargetOfSkill target = DrawEnum<TargetOfSkill>(P_TARGET);

                    Space();
                    DrawInt(P_COST, 1, 4, 1);

                    Space(2f);
                    DrawLabel("UI:");
                    indentLevel++;
                    DrawStringPopup(keyNameProperty, KEYS_NAME_SKILLS);
                    DrawObjectRelative<Sprite>(uiProperty, P_SPRITE_UI, true);
                    indentLevel--;

                    DrawLine(40f);
                    indentLevel--;
                    DrawProperty(P_SFX, "SFX Name");
                    DrawHits(hitsCount, target);
                    indentLevel++;
                }

                indentLevel--;
            }

            indentLevel--;
            EndProperty();

            #region Local: SetName(..)
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
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _height * GetPropertyRate(property);
        }

    }
}
