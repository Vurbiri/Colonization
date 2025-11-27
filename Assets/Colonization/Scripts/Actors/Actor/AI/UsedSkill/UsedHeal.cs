using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class UsedHeal
    {
        [SerializeField] private int _heal;
        [SerializeField] private bool _cure;
        [SerializeField] private bool _usesSelfHP;
        
        public bool IsValid { [Impl(256)] get => _heal >= 0; }

        [Impl(256)] public bool CanUsed(Actor user, Actor target) => user.Action.CanUsedSkill(_heal) && ChanceValue(user, target) > 0;

        public IEnumerator TryUse_Cn(Actor user, Actor target)
        {
            if (user.Action.CanUsedSkill(_heal) && Chance.Rolling(ChanceValue(user, target)))
            {
                yield return GameContainer.CameraController.ToPositionControlled(target);
                yield return user.UseSkill_Cn(target, _heal);
            }
            yield break;
        }

        [Impl(256)] public int ChanceValue(Actor user, Actor target)
        {
            int targetHP = target.PercentHP;
            int baseHP = _usesSelfHP ? user.PercentHP : 100;
            baseHP = _cure && target.Effects.ContainsNegative() ? baseHP * 5 >> 2 : baseHP;

            return (baseHP * baseHP - targetHP * targetHP) / 100;
        }

#if UNITY_EDITOR
        public const string skillField = nameof(_heal);
        public const string cureField = nameof(_cure);
        public const string usesSelfHPField = nameof(_usesSelfHP);
#endif
    }
}

