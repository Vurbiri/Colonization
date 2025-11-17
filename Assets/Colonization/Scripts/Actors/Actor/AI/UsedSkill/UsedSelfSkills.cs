using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class UsedSelfSkills
	{
        [SerializeField] private UsedSelfSkill[] _skills;

        public IEnumerator TryUse_Cn(Actor target)
        {
            for(int i = 0; i < _skills.Length; ++i)
                yield return _skills[i].TryUse(target);
            yield return null;
        }

        #region UsedSelfSkill
        [Serializable]
        private struct UsedSelfSkill
        {
            public int skill;
            public MinMaxHP currentHP;
            public Chance chance;

            public WaitSignal TryUse(Actor target)
            {
                var action = target.Action;
                if(currentHP.IsValid(target) && action.CanUsedSkill(skill) && !action.IsApplied(skill, target) && chance.Roll)
                    return action.UseSkill(skill);
                return null;
            }
        }
        #endregion
    }
}
