using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public enum LinkListMode
    {
        First,
        Last
    }

    public class LinkList<T> : IReadOnlyCollection<T>, IEnumerable<T>
    {
        public T First => _first.value;
        public T Last => _last.value;

        public LinkListMode Mode { get => _addMode; set { _addMode = value; _actionAdd = _addMode == LinkListMode.Last ? AddLast : AddFirst; } }
        public int Count => _count;

        private int _count;
        private LinkListNode _first, _last;
        private LinkListMode _addMode;
        private Action<T> _actionAdd;

        public LinkList()
        {
            _addMode = LinkListMode.Last;
            _actionAdd = AddLast;
            _count = 0;
        }

        public void AddLast(T value)
        {
            LinkListNode temp = new(value, _last, null);
            if (_count > 0)
                _last.next = temp;
            else
                _first = temp;
            _last = temp;
            _count++;
        }
        public void AddFirst(T value)
        {
            LinkListNode temp = new(value, null, _first);
            if (_count > 0)
                _first.prev = temp;
            else
                _last = temp;
            _first = temp;
            _count++;
        }
        public void Add(T value) => _actionAdd(value);
        public void Add(params T[] values) => AddRang(values);
        public void AddRang(IEnumerable<T> values)
        {
            foreach (T value in values)
                _actionAdd(value);
        }

        public bool Union(LinkList<T> other)
        {
            if (_last.EqualsValue(other._first))
                AddFirstToLast();
            else if (_first.EqualsValue(other._last))
                AddLastToFirst();
            else if (_first.EqualsValue(other._first))
                AddFirstToFirst();
            else if (_last.EqualsValue(other._last))
                AddLastToLast();
            else
                return false;

            return true;

            #region Local: AddFirstToLast(), AddLastToFirst(), AddFirstToFirst(), AddLastToLast()
            //=================================
            void AddFirstToLast()
            {
                _last.next = other._first.next;
                other._first.next.prev = _last;
                _last = other._last;
                _count += other._count - 1;
            }
            //=================================
            void AddLastToFirst()
            {
                _first.prev = other._last.prev;
                other._last.prev.next = _first;
                _first = other._first;
                _count += other._count - 1;
            }
            //=================================
            void AddFirstToFirst()
            {
                other.Reverse();
                AddLastToFirst();
            }
            //=================================
            void AddLastToLast()
            {
                other.Reverse();
                AddFirstToLast();
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
            LinkListNode current = _first;

            while (current != null)
            {
                yield return current.value;
                current = current.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Reverse()
        {
            LinkListNode temp = _last;

            while (temp != null)
            {
                temp.Revers();
                temp = temp.next;
            }
            (_first, _last) = (_last, _first);
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