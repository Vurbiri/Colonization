using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable, JsonArray]
	public class ChanceUsedSkills<TId> : IEnumerable<ChanceUsedSkill> where TId : ActorId<TId>
	{
		[SerializeField] private ChanceUsedSkill[] _values;

        public ChanceUsedSkill this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; }
        public ChanceUsedSkill this[int index] { [Impl(256)] get => _values[index]; }

        private ChanceUsedSkills() { }

        [JsonConstructor]
        protected ChanceUsedSkills(IReadOnlyList<ChanceUsedSkill> list)
        {
            _values = new ChanceUsedSkill[IdType<TId>.Count];
            int count = Mathf.Min(IdType<TId>.Count, list.Count);
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }

        public IEnumerator<ChanceUsedSkill> GetEnumerator() => new ArrayEnumerator<ChanceUsedSkill>(_values, ActorId<TId>.Count);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<ChanceUsedSkill>(_values, ActorId<TId>.Count);
    }
}
