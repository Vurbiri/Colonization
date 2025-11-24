using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class UsedDebuffs
	{
        [SerializeField, JsonProperty] private UsedDebuff[] _skills;

        public bool CanUsed(Actor user, Actor target)
        {
            var action = user.Action;
            for (int i = _skills.Length - 1; i >= 0; --i)
                if (_skills[i].CanUsed(action, target))
                    return true;
            return false;
        }

        public IEnumerator TryUse_Cn(Actor user, Actor target)
        {
            for (int i = 0; i < _skills.Length; ++i)
                yield return _skills[i].TryUse_Cn(user, target);
            yield break;
        }

        [Serializable] private struct UsedDebuff
        {
#pragma warning disable 649
            public int skill;
            public Chance chance;
#pragma warning restore

            [Impl(256)] public readonly bool CanUsed(Actor.Actions action, Actor target) => action.CanUsedSkill(skill) && !action.IsApplied(skill, target);

            public IEnumerator TryUse_Cn(Actor user, Actor target)
            {
                if (CanUsed(user.Action, target) && chance.Roll)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(target);
                    yield return user.UseSkill_Cn(target, skill);
                }
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
