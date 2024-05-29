using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
    public static Vector2Int Rand => line.Rand();
    public static IEnumerable<Vector2Int> LineRange => new Direction2DEnumerable(line);
    private static readonly Vector2Int[] line =
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    };

    public static IEnumerable<Vector2Int> ExcludingRange(Vector2Int direction) 
    {
        if (direction.x > 0) return new Direction2DEnumerable(directThreeExcluding[0]);
        if (direction.x < 0) return new Direction2DEnumerable(directThreeExcluding[1]);
        if (direction.y > 0) return new Direction2DEnumerable(directThreeExcluding[2]);
        if (direction.y < 0) return new Direction2DEnumerable(directThreeExcluding[3]);

        return new Direction2DEnumerable(line);
    }
    private static readonly Vector2Int[][] directThreeExcluding =
    {
        new Vector2Int[]{ Vector2Int.down, Vector2Int.left, Vector2Int.up },
        new Vector2Int[]{ Vector2Int.up, Vector2Int.right, Vector2Int.down },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.down, Vector2Int.left },
        new Vector2Int[]{ Vector2Int.left, Vector2Int.up, Vector2Int.right },
    };

    public static IEnumerable<Vector2Int> ExcludingRange(Vector2Int directionA, Vector2Int directionB)
    {
        HashSet<Vector2Int> indexes = new(line);

        if (directionA.x > 0 || directionB.x > 0) indexes.Remove(Vector2Int.right);
        if (directionA.x < 0 || directionB.x < 0) indexes.Remove(Vector2Int.left);
        if (directionA.y > 0 || directionB.y > 0) indexes.Remove(Vector2Int.up);
        if (directionA.y < 0 || directionB.y < 0) indexes.Remove(Vector2Int.down);

        return new Direction2DEnumerable(indexes);
    }

    #region Nested classes
    //*************************************************************
    private class Direction2DEnumerable : IEnumerable<Vector2Int>
    {
        private readonly Direction2DEnumerator _enumerator;

        public Direction2DEnumerable(Vector2Int[] direct) => _enumerator = new(direct);
        public Direction2DEnumerable(ICollection<Vector2Int> direct) => _enumerator = new(direct);

        public IEnumerator<Vector2Int> GetEnumerator() => _enumerator;

        IEnumerator IEnumerable.GetEnumerator() => _enumerator;
    }
    //*************************************************************
    private class Direction2DEnumerator : IEnumerator<Vector2Int>
    {
        private readonly Vector2Int[] _direct;
        private readonly byte[] _indexes;
        private readonly int _count;
        private int _cursor;
        private Vector2Int _current;

        public Direction2DEnumerator(Vector2Int[] direct)
        {
            _direct = direct;
            _cursor = -1;
            _current = default;
            _count = direct.Length;

            _indexes = _count switch 
            {
                2 => two.Rand(),
                3 => three.Rand(),
                4 => four.Rand(),
                _ => one
            };
        }

        public Direction2DEnumerator(ICollection<Vector2Int> direct)
        {
            _count = direct.Count;
            _direct = new Vector2Int[_count];
            direct.CopyTo(_direct, 0);
            _cursor = -1;
            _current = default;

            _indexes = _count switch
            {
                2 => two.Rand(),
                3 => three.Rand(),
                4 => four.Rand(),
                _ => one
            };
        }

        public Vector2Int Current => _current;
        object IEnumerator.Current => _current;

        public bool MoveNext()
        {
            if (++_cursor >= _count)
                return false;

            _current = _direct[_indexes[_cursor]];
            return true;
        }

        public void Reset() => _cursor = -1;

        public void Dispose() { }

        private static readonly byte[] one = { 0 };

        private static readonly byte[][] two =
        {
            new byte[]{ 0, 1 },
            new byte[]{ 1, 0 },
        };

        private static readonly byte[][] three =
        {
            new byte[]{ 0, 1, 2 },
            new byte[]{ 0, 2, 1 },
            new byte[]{ 1, 0, 2 },
            new byte[]{ 1, 2, 0 },
            new byte[]{ 2, 0, 1 },
            new byte[]{ 2, 1, 0 },
        };

        private static readonly byte[][] four =
        {
            new byte[]{ 0, 1, 2, 3 },
            new byte[]{ 0, 1, 3, 2 },
            new byte[]{ 0, 2, 1, 3 },
            new byte[]{ 0, 2, 3, 1 },
            new byte[]{ 0, 3, 1, 2 },
            new byte[]{ 0, 3, 2, 1 },

            new byte[]{ 1, 0, 2, 3 },
            new byte[]{ 1, 0, 3, 2 },
            new byte[]{ 1, 2, 0, 3 },
            new byte[]{ 1, 2, 3, 0 },
            new byte[]{ 1, 3, 2, 0 },
            new byte[]{ 1, 3, 0, 2 },

            new byte[]{ 2, 0, 1, 3 },
            new byte[]{ 2, 0, 3, 1 },
            new byte[]{ 2, 1, 0, 3 },
            new byte[]{ 2, 1, 3, 0 },
            new byte[]{ 2, 3, 1, 0 },
            new byte[]{ 2, 3, 0, 1 },
        };
    }
#endregion
}
