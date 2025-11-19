using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class UsedDebuffs
	{
        [SerializeField, JsonProperty] private UsedDebuff[] _skills;

        public IEnumerator TryUse_Cn(Actor user, Actor target)
        {
            for (int i = 0; i < _skills.Length; ++i)
                yield return _skills[i].TryUse_Cn(user, target);
            yield break;
        }

        [Serializable] private struct UsedDebuff
        {
            public int skill;
            public Chance chance;

            public IEnumerator TryUse_Cn(Actor user, Actor target)
            {
                var action = user.Action;
                if (action.CanUsedSkill(skill) && !action.IsApplied(skill, target) && chance.Roll)
                    yield return user.UseSkill_Cn(target, skill);
                yield break;
            }
        }

#if UNITY_EDITOR
        public const string arrayField = nameof(_skills);

        public const string skillField = nameof(UsedDebuff.skill);
        public const string chanceField = nameof(UsedDebuff.chance);
#endif
    }
}
