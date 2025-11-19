using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable, JsonObject(MemberSerialization.Fields)]
    public struct MinMaxHP
	{
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        [Impl(256)] public readonly bool IsValid(Actor actor)
        {
            int current = actor.PercentHP;
            return current >= _min && current <= _max;
        }
	}
}
