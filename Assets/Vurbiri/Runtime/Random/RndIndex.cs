using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public struct RndIndex
    {
        private readonly int _start;
        private readonly int _count;
        private int _current;

        public readonly int Start { [Impl(256)] get => _start; }
        public readonly int Count { [Impl(256)] get => _count; }
        public readonly int Current { [Impl(256)] get => _current; }
        public int Next { [Impl(256)] get => _current = (_current + Random.Range(_start, _count)) % _count; }

        [Impl(256)] public RndIndex(int count) : this(0, count) { }
        [Impl(256)] public RndIndex(int start, int count)
        {
            Throw.IfIndexOutOfRange(start, count);

            _start = start;
            _count = count;
            _current = Random.Range(start, count);
        }

        [Impl(256)] public static implicit operator int(RndIndex index) => index._current;
    }
}
