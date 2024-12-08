//Assets\Colonization\Editor\Actors\Utility\ActorUtility.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.Skills;

namespace VurbiriEditor.Colonization.Actors
{
    public class ActorUtility
	{
        public static readonly string[] A_SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };

        public static void OverrideClips(IReadOnlyList<ActorSettings> list)
        {
            Debug.Log("[OverrideClips]");

            ActorSkin actorSkin;
            AnimatorOverrideController animator;
            AActorSFX actorSFX;
            Skills skills;
            SkillSettings skillSettings;
            AnimationClipSettingsScriptable clipSettings;

            foreach (var settings in list)
            {
                actorSkin = settings.Skin;
                if (actorSkin == null)
                    continue;

                animator = (AnimatorOverrideController)actorSkin.GetComponent<Animator>().runtimeAnimatorController;
                actorSFX = actorSkin.GetComponent<AActorSFX>();
                skills = settings.Skills;
                int count = skills.List.Count, i;

                actorSkin.SetCountAnimationClips(count);
                actorSFX.SetCountSkillsSFX(count);

                for (i = 0; i < count; i++)
                {
                    skillSettings = skills.List[i];
                    clipSettings = skillSettings.clipSettings;

                    if (animator[A_SKILLS[i]] != clipSettings.clip)
                        animator[A_SKILLS[i]] = clipSettings.clip;

                    actorSkin.SetAnimationClip(clipSettings, i);
                    actorSFX.SetSkillSFX(skillSettings.skillSFX, clipSettings.totalTime, i);

                    skillSettings.range = clipSettings.range;
                }

                for (; i < COUNT_SKILLS_MAX; i++)
                    if (animator[A_SKILLS[i]] != null)
                        animator[A_SKILLS[i]] = null;
            }
        }
    }
}
