using Newtonsoft.Json;
using System;
using UnityEngine;
using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class UsedSelfSkill
    {
        [SerializeField] private int _skill;
        [SerializeField] private Chance _chance;

        [Impl(256)] public bool CanUsed(Actor target)
        {
            var action = target.Action;
            return action.CanUsedSkill(_skill) && !action.IsApplied(_skill, target) && _chance.Roll;
        }
        [Impl(256)] public WaitSignal Use(AStates action) => action.UseSkill(_skill);
    }
}
