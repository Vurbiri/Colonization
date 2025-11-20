using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public struct SkillApplied
	{
        [SerializeField] private bool _maxAP;
        [SerializeField] private bool _onlyAntipode;
        [SerializeField] private bool _self;
        [SerializeField] private bool _target;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsValid(Actor user, Actor target, int skillId)
        {
            var action = user.Action;
            return (!_maxAP || user.IsMaxAP) && 
                  !((_onlyAntipode && user.TypeId == target.TypeId) || (_self && action.IsApplied(skillId, user)) || (_target && action.IsApplied(skillId, target)));
        }

#if UNITY_EDITOR
        public const string antipodeField = nameof(_onlyAntipode);
        public const string maxAPField = nameof(_maxAP);
        public const string selfField = nameof(_self);
        public const string targetField = nameof(_target);
#endif
    }
}
