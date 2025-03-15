//Assets\Vurbiri\Runtime\Types\Random\RIndex.cs
using UnityEngine;

namespace Vurbiri
{
    public struct RIndex
    {
        private readonly int _count;
        private int _current;

        public readonly int Index => _current;
        public readonly int Count => _count;
        public int Roll => _current = (_current + Random.Range(0, _count)) % _count;

        public RIndex(int count)
        {
            _count = count;
            _current = Random.Range(0, _count);
        }

        public void Next() => _current = (_current + Random.Range(0, _count)) % _count;

        public static implicit operator int(RIndex index) => index._current;
        public static implicit operator RIndex(int count) => new(count);

        public override readonly string ToString() => $"(Current index: {_current}. Count: {_count})";
    }
}
