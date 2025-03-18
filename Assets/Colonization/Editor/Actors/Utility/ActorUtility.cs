//Assets\Colonization\Editor\Actors\Utility\ActorUtility.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.Skills;

namespace VurbiriEditor.Colonization.Actors
{
    public class ActorUtility
	{
        public static readonly string[] A_SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };

        public static void OverrideClips(IReadOnlyList<ActorSettings> listSettings)
        {
            Debug.Log("[OverrideClips]");

            ActorSkin actorSkin;
            AnimatorOverrideController animator;
            AActorSFX actorSFX;
            Skills skills;
            SkillSettings skillSettings;
            AnimationClipSettingsScriptable clipSettings;

            foreach (var settings in listSettings)
            {
                actorSkin = settings.PrefabSkin;
                if (actorSkin == null)
                    continue;

                animator = (AnimatorOverrideController)actorSkin.GetComponent<Animator>().runtimeAnimatorController;
                actorSFX = actorSkin.GetComponent<AActorSFX>();
                skills = settings.Skills;
                int countSkills = skills.Settings.Count, i;

                actorSkin.SetCountSkills_EditorOnly(countSkills);
                actorSFX.SetCountSkillsSFX_EditorOnly(countSkills);

                for (i = 0; i < countSkills; i++)
                {
                    skillSettings = skills.Settings[i];
                    clipSettings = skillSettings.clipSettings;

                    if (animator[A_SKILLS[i]] != clipSettings.clip)
                        animator[A_SKILLS[i]] = clipSettings.clip;

                    int countHits = skillSettings.SFXHits.Length;
                    actorSFX.SetCountHitsSFX_EditorOnly(i, countHits);

                    for (int j = 0; j < countHits; j++)
                    {
                        actorSFX.SetSkillSFX_EditorOnly(i, j, skillSettings.SFXHits[j]);
                    }

                    actorSkin.SetTimings_EditorOnly(clipSettings, i);
                }

                for (; i < COUNT_SKILLS_MAX; i++)
                    if (animator[A_SKILLS[i]] != null)
                        animator[A_SKILLS[i]] = null;

                PrefabUtility.SavePrefabAsset(actorSkin.gameObject);
            }

            AssetDatabase.SaveAssets();
        }
    }
}
