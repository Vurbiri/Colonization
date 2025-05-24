using UnityEngine;

namespace Vurbiri
{
    public struct IndexRnd
    {
        private readonly int _start;
        private readonly int _count;
        private int _current;

        public readonly int Current => _current;
        public readonly int Start => _start;
        public readonly int Count => _count;

        public IndexRnd(int count) : this(0, count) { }
        public IndexRnd(int start, int count)
        {
            Throw.IfIndexOutOfRange(start, count);

            _start = start;
            _count = count;
            _current = Random.Range(start, count);
        }

        public int Next() => _current = (_current + Random.Range(_start, _count)) % _count;

        public static explicit operator IndexRnd(int count) => new(count);
        public static implicit operator int(IndexRnd index) => index._current;

        public override readonly string ToString() => $"(Current index: {_current}. Count: {_count})";
    }
}
