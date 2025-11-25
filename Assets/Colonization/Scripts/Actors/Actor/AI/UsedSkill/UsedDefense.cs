using Newtonsoft.Json;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class UsedDefense
	{
        [SerializeField] private int _buff;
        [SerializeField] private Chance _buffChance;
        [SerializeField] private bool _block;
        [SerializeField] private Chance _blockChance;

        public bool IsValid => _buff >= 0 | _block;

        public (bool, bool) CanUsed(Actor target, bool notRoll = false)
        {
            var action = target.Action;
            return (CanBuff(action, target, notRoll), CanBlock(action, notRoll));
        }

        public IEnumerator Use_Cn(Actor actor, bool isBuff, bool isBlock)
        {
            var action = actor.Action;
            if (isBuff)
                yield return action.UseSkill(_buff);
            if (isBlock && (!isBuff || action.CanUsedSpecSkill()))
                yield return action.UseSpecSkill();
            yield break;
        }

        public IEnumerator TryUse_Cn(Actor actor, bool isBuff, bool isBlock, bool notRoll = false)
        {
            var action = actor.Action;
            return Use_Cn(actor, isBuff && CanBuff(action, actor, notRoll), isBlock && CanBlock(action, notRoll));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanBuff(Actor.Actions action, Actor target, bool notRoll)
        {
            return action.CanUsedSkill(_buff) && !action.IsApplied(_buff, target) && (notRoll || _buffChance.Roll);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanBlock(Actor.Actions action, bool notRoll)
        {
            return _block && action.CanUsedSpecSkill() && (notRoll || _blockChance.Roll);
        }

#if UNITY_EDITOR
        public const string buffField = nameof(_buff);
        public const string buffChanceField = nameof(_buffChance);
        public const string blockField = nameof(_block);
        public const string blockChanceField = nameof(_blockChance);
#endif
    }
}
