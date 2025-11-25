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

        public bool IsValid => _heal >= 0;

        public bool CanUsed(Actor user, Actor target) => user.Action.CanUsedSkill(_heal) && ChanceValue(user, target) > 0;

        public IEnumerator TryUse_Cn(Actor user, Actor target)
        {
            if (user.Action.CanUsedSkill(_heal) && Chance.Rolling(ChanceValue(user, target)))
            {
                yield return GameContainer.CameraController.ToPositionControlled(target);
                yield return user.UseSkill_Cn(target, _heal);
            }
            yield break;
        }

        public int ChanceValue(Actor user, Actor target) => (_useSelfHP ? user.PercentHP - _maxHP : _maxHP) - target.PercentHP;

#if UNITY_EDITOR
        public const string skillField = nameof(_heal);
        public const string maxHPField = nameof(_maxHP);
        public const string useSelfHPField = nameof(_useSelfHP);
#endif
    }
}

