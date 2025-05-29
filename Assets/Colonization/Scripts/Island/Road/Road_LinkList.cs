using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{

    public partial class Road
    {
        private class Points
        {
            public const float OFFSET_Y = 0.0125f;

            private readonly LineRenderer _roadRenderer;

            private IntRnd _rangeCount = new(3, 6);
            private FloatRnd _rateWave = new(0.12f, 0.24f);
            private FloatRnd _lengthFluctuation = new(0.85f, 1.15f);

            private Vector3[] _values;
            private int _capacity = 4;
            private int _count;

            public Points(LineRenderer roadRenderer, Vector3 start, Vector3 end)
            {
                _values = new Vector3[_capacity];
                _roadRenderer = roadRenderer;

                start.y = OFFSET_Y;
                _values[0] = start;
                _count = 1;

                Add(start, end);
            }


            public void Add(Vector3 start, Vector3 end)
            {
                int addCount = _rangeCount, nextCount = _count + addCount;

                if (nextCount > _capacity) 
                    GrowArray(nextCount);

                start.y = end.y = OFFSET_Y;
                Vector3 step = (end - start) / addCount, offsetSide = Vector3.Cross(Vector3.up, step.normalized);
                float sign = Chance.Select(1f, -1f), signStep = -1f;

                nextCount--;
                for (int i = _count; i < nextCount; i++)
                {
                    sign *= signStep;
                    _values[i] = start += _lengthFluctuation * step + _rateWave * sign * offsetSide;
                }
                _values[nextCount] = end;
                _count = nextCount + 1;

                SetPositions();
            }

            public void Insert(Vector3 start, Vector3 end)
            {
                int addCount = _rangeCount, nextCount = _count + addCount;

                if (nextCount > _capacity)
                {
                    GrowArray(nextCount, addCount);
                }
                else
                {
                    for (int i = nextCount - 1, j = _count - 1; j >= 0; i--, j--)
                        _values[i] = _values[j];
                }

                start.y = end.y = OFFSET_Y;
                Vector3 step = (end - start) / addCount, offsetSide = Vector3.Cross(Vector3.up, step.normalized);
                float sign = Chance.Select(1f, -1f), signStep = -1f;

                for (int i = addCount - 1; i > 0; i--)
                {
                    sign *= signStep;
                    _values[i] = start += _lengthFluctuation * step + _rateWave * sign * offsetSide;
                }
                _values[0] = end;
                _count = nextCount;

                SetPositions();
            }

            private void SetPositions()
            {
                _roadRenderer.positionCount = _count;
                _roadRenderer.SetPositions(_values);
            }

            private void GrowArray(int count, int offset = 0)
            {
                while (_capacity < count)
                    _capacity = _capacity << 1 | 1 << 5;

                Debug.Log($"GrowArray {offset}");

                Vector3[] array = new Vector3[_capacity];
                for (int i = 0, j = offset; i < _count; i++, j++)
                    array[j] = _values[i];
                _values = array;
            }

            internal bool Union(Points points)
            {
                return false;
            }
        }


        

        private class LinkList<T> : IReadOnlyCollection<T>, IEnumerable<T>
        {
            public T Zero => _zero.value;
            public T End => _end.value;

            public LinkListMode Mode { get => _addMode; set { _addMode = value; _actionAdd = _addMode == LinkListMode.End ? AddToEnd : AddToZero; } }
            public int Count => _count;

            private int _count;
            private LinkListNode _zero, _end;
            private LinkListMode _addMode;
            private Action<T> _actionAdd;

            public LinkList()
            {
                _addMode = LinkListMode.End;
                _actionAdd = AddToEnd;
                _count = 0;
            }

            public void AddToEnd(T value)
            {
                LinkListNode temp = new(value, _end, null);
                if (_count > 0)
                    _end.next = temp;
                else
                    _zero = temp;
                _end = temp;
                _count++;
            }
            public void AddToZero(T value)
            {
                LinkListNode temp = new(value, null, _zero);
                if (_count > 0)
                    _zero.prev = temp;
                else
                    _end = temp;
                _zero = temp;
                _count++;
            }
            public void Add(T value) => _actionAdd(value);
            public void Add(T valueA, T valueB)
            {
                _actionAdd(valueA);
                _actionAdd(valueB);
            }

            public bool Union(LinkList<T> other)
            {
                if (_end.EqualsValue(other._zero))
                    AddFirstToLast(other);
                else if (_zero.EqualsValue(other._end))
                    AddLastToFirst(other);
                else if (_zero.EqualsValue(other._zero))
                    AddFirstToFirst(other);
                else if (_end.EqualsValue(other._end))
                    AddLastToLast(other);
                else
                    return false;

                return true;

                #region Local: AddFirstToLast(...), AddLastToFirst(...), AddFirstToFirst(...), AddLastToLast(...)
                //=================================
                void AddFirstToLast(LinkList<T> other)
                {
                    _end.next = other._zero.next;
                    other._zero.next.prev = _end;
                    _end = other._end;
                    _count += other._count - 1;
                }
                //=================================
                void AddLastToFirst(LinkList<T> other)
                {
                    _zero.prev = other._end.prev;
                    other._end.prev.next = _zero;
                    _zero = other._zero;
                    _count += other._count - 1;
                }
                //=================================
                void AddFirstToFirst(LinkList<T> other)
                {
                    other.Reverse();
                    AddLastToFirst(other);
                }
                //=================================
                void AddLastToLast(LinkList<T> other)
                {
                    other.Reverse();
                    AddFirstToLast(other);
                }
                #endregion
            }

            public T[] ToArray()
            {
                T[] array = new T[_count];
                int i = 0;
                foreach (var item in this)
                    array[i++] = item;
                return array;
            }

            public IEnumerator<T> GetEnumerator()
            {
                LinkListNode current = _zero;

                while (current != null)
                {
                    yield return current.value;
                    current = current.next;
                }
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private void Reverse()
            {
                LinkListNode temp = _end;

                while (temp != null)
                {
                    temp.Revers();
                    temp = temp.next;
                }
                (_zero, _end) = (_end, _zero);
            }

            #region Nested class: LinkListNode
            private class LinkListNode
            {
                public T value;

                public LinkListNode prev;
                public LinkListNode next;

                public LinkListNode(T value, LinkListNode prev, LinkListNode next)
                {
                    this.value = value;
                    this.prev = prev;
                    this.next = next;
                }

                public void Revers() => (prev, next) = (next, prev);

                public bool EqualsValue(LinkListNode other) => value.Equals(other.value);
            }
            #endregion
        }
    }
}
