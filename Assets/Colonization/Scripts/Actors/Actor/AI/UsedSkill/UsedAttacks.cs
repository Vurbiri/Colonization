using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class UsedAttacks
	{
        [SerializeField, JsonProperty] private UsedAttack[] _skills;

        private Attacks _attacks;

        public bool CanUsed(Actor user, Actor target)
        {
            for (int i = _skills.Length - 1; i >= 0; --i)
                if (_skills[i].CanUsed(user, target))
                    return true;
            return false;
        }

        public IEnumerator TryUse_Cn(Actor user, Actor target)
        {
            UsedAttack attack;
            while (_attacks.Count > 0)
            {
                attack = _attacks.RandomExtract();
                if (attack.ChanceUse(user, target))
                {
                    yield return GameContainer.CameraController.ToPositionControlled(target);
                    yield return user.UseSkill_Cn(target, attack.skill);
                    break;
                }
            }

            _attacks.Restore(_skills);
            yield break;
        }

        public void Init() => _attacks = new(_skills);

        #region Nested: UsedAttack, Attacks
        //*********************************
        [Serializable]
        private class UsedAttack : IEquatable<UsedAttack>
        {
            public int skill;
            public Chance chance;
            public SkillApplied applied;
            public MinMaxHP selfHP;
            public MinMaxHP targetHP;
            
            [Impl(256)] public bool CanUsed(Actor user, Actor target)
            {
                return user.Action.CanUsedSkill(skill) && applied.IsValid(user, target, skill) && selfHP.IsValid(user) && targetHP.IsValid(target);
            }
            [Impl(256)] public bool ChanceUse(Actor user, Actor target) => CanUsed(user, target) && chance.Roll;

            public bool Equals(UsedAttack other) => other is not null && other.skill == skill;
        }
        //*********************************
        private class Attacks : List<UsedAttack>
        {
            public Attacks(UsedAttack[] skills) : base(skills.Length)
            {
                for (int i = skills.Length - 1; i >= 0; --i)
                    Add(skills[i]);
            }

            public void Restore(UsedAttack[] skills)
            {
                UsedAttack skill;
                for (int i = skills.Length - 1; i >= 0; --i)
                {
                    skill = skills[i];
                    if(IndexOf(skill) < 0)
                        Add(skill);
                }
            }
        }
        //*********************************
        #endregion

#if UNITY_EDITOR
        public const string arrayField = nameof(_skills);

        public const string skillField = nameof(UsedAttack.skill);
        public const string selfHPField = nameof(UsedAttack.selfHP);
        public const string targetHPField = nameof(UsedAttack.targetHP);
        public const string appliedField = nameof(UsedAttack.applied);
        public const string chanceField = nameof(UsedAttack.chance);
#endif
    }
}
