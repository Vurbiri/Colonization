using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class UsedHeal
    {
        [SerializeField] private int _heal;
        [SerializeField] private int _maxHP;
        [SerializeField] private bool _useSelfHP;

        public IEnumerator TryUse_Cn(Actor user, Actor target)
        {
            var action = user.Action;
            if (action.CanUsedSkill(_heal) && Chance.Rolling((_useSelfHP ? user.PercentHP - _maxHP : _maxHP) - target.PercentHP))
                yield return user.UseSkill_Cn(target, _heal);
            yield break;
        }

#if UNITY_EDITOR
        public const string skillField = nameof(_heal);
        public const string maxHPField = nameof(_maxHP);
        public const string useSelfHPField = nameof(_useSelfHP);
#endif
    }
}

