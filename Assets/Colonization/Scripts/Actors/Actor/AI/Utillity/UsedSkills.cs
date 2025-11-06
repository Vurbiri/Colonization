using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[System.Serializable, Newtonsoft.Json.JsonArray]
	public class UsedSkills<TId> : IEnumerable<int> where TId : ActorId<TId>
	{
		[SerializeField] private int[] _values;

        public int this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; }
        public int this[int index] { [Impl(256)] get => _values[index]; }

        private UsedSkills() { }

        [Newtonsoft.Json.JsonConstructor]
        private UsedSkills(IReadOnlyList<int> list)
        {
            _values = new int[IdType<TId>.Count];
            int count = Mathf.Min(IdType<TId>.Count, list.Count);
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }

        public IEnumerator<int> GetEnumerator() => new ArrayEnumerator<int>(_values, ActorId<TId>.Count);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<int>(_values, ActorId<TId>.Count);
    }
}
