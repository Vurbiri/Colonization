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
        public static void OverrideClips(IReadOnlyList<ActorSettings> list)
        {
            Debug.Log("[OverrideClips]");
            
            AnimatorOverrideController animator;
            Skills skills;
            SkillSettings skillSettings;

            foreach (var settings in list)
            {
                if (settings.Skin == null)
                    continue;

                animator = (AnimatorOverrideController)settings.Skin.GetComponent<Animator>().runtimeAnimatorController;
                skills = settings.Skills;
                int count = skills.List.Count;
                settings.Skin.SetCountAnimationClips(count);
                for (int i = 0; i < count; i++)
                {
                    skillSettings = skills.List[i];

                    settings.Skin.SetAnimationClip(skillSettings.clipSettings, i);
                    skillSettings.range = skillSettings.clipSettings.range;
                }
            }
        }
    }
}
