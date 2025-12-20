using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class UsedSelfBuffs
	{
        [SerializeField, JsonProperty] private UsedSelfBuff[] _skills;

        public IEnumerator TryUse_Cn(Actor target)
        {
            for(int i = 0; i < _skills.Length; ++i)
                yield return _skills[i].TryUse(target);
            yield break;
        }

        [Serializable] private struct UsedSelfBuff
        {
#pragma warning disable 649
            public int skill;
            public MinMaxHP currentHP;
            public Chance chance;
#pragma warning restore 649

            public IEnumerator TryUse(Actor target)
            {
                var action = target.Action;
                if (currentHP.IsValid(target) && action.CanUsedSkill(skill) && !action.IsApplied(skill, target) && chance.Roll)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(target);
                    yield return action.UseSkill(skill);
                }
                yield break;
            }
        }

#if UNITY_EDITOR
        public const string arrayField = nameof(_skills);

        public const string skillField = nameof(UsedSelfBuff.skill);
        public const string currentHPField = nameof(UsedSelfBuff.currentHP);
        public const string chanceField = nameof(UsedSelfBuff.chance);
#endif
    }
}
