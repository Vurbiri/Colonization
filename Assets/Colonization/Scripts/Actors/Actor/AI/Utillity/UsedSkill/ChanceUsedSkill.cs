using Newtonsoft.Json;
using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class ChanceUsedSkill
    {
        [SerializeField] private int _skill;
        [SerializeField] private Chance _chance;

        [Impl(256)] public bool CanUsed<TAction>(TAction action) where TAction : Actor.Actions => action.CanUsedSkill(_skill) && _chance.Roll;
        [Impl(256)] public bool CanUsed<TAction>(TAction action, Actor target) where TAction : Actor.Actions
        {
            return action.CanUsedSkill(_skill) && !action.IsApplied(_skill, target) && _chance.Roll;
        }
        [Impl(256)] public WaitSignal Use<TAction>(TAction action) where TAction : Actor.Actions => action.UseSkill(_skill);
    }
}
