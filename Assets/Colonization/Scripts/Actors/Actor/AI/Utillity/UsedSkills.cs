using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class UsedSkill
    {
        public int skill;
        public Chance chance;
    }

    [Serializable, JsonArray]
	public class UsedSkills<TId> : IEnumerable<UsedSkill> where TId : ActorId<TId>
	{
		[SerializeField] private UsedSkill[] _values;

        public UsedSkill this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; }
        public UsedSkill this[int index] { [Impl(256)] get => _values[index]; }

        private UsedSkills() { }

        [JsonConstructor]
        protected UsedSkills(IReadOnlyList<UsedSkill> list)
        {
            _values = new UsedSkill[IdType<TId>.Count];
            int count = Mathf.Min(IdType<TId>.Count, list.Count);
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }

        public IEnumerator<UsedSkill> GetEnumerator() => new ArrayEnumerator<UsedSkill>(_values, ActorId<TId>.Count);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<UsedSkill>(_values, ActorId<TId>.Count);
    }
}
