using Newtonsoft.Json;
using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class UsedHeal : UsedTargetSkill
    {
        [SerializeField] private int _maxHP;
        [SerializeField] private bool _useSelfHP;

        [Impl(256)] public bool CanUsed(Actor user, Actor target)
        {
            return user.Action.CanUsedSkill(_skill) && Chance.Rolling((_useSelfHP ? user.PercentHP - _maxHP : _maxHP) - target.PercentHP);
        }
    }
}

