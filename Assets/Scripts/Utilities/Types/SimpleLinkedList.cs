using System;
using System.Collections;
using System.Collections.Generic;

public class SimpleLinkedList<T> : IReadOnlyCollection<T>, IEnumerator<T>
{
    public T First => _first.value;
    public T Last => _last.value;
    
    public bool ModeLast { get => _modeLast; set { _modeLast = value; _actionAdd = _modeLast ? AddLast : AddFirst; } }
    public bool ModeFirst { get => !_modeLast; set => ModeLast = !value; }
    public int Count => _count;

    private int _count;
    private LinkListNode _first, _last, _current;
    private bool _modeLast;
    private Action<T> _actionAdd;

    public SimpleLinkedList()
    {
        _modeLast = true;
        _actionAdd = AddLast;
    }

    public void AddLast(T value)
    {
        LinkListNode temp = new(value, _last, null);
        if(_count > 0)
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

    public void AddFirstToLast(SimpleLinkedList<T> other)
    {
        _last.next = other._first.next;
        other._first.next.prev = _last;
        _last = other._last;
        _count += other._count - 1;
    }
    public void AddLastToFirst(SimpleLinkedList<T> other)
    {
        _first.prev = other._last.prev;
        other._last.prev.next = _first;
        _first = other._first;
        _count += other._count - 1;
    }
    public void AddFirstToFirst(SimpleLinkedList<T> other)
    {
        other.Reverse();
        AddLastToFirst(other);
    }
    public void AddLastToLast(SimpleLinkedList<T> other)
    {
        other.Reverse();
        AddFirstToLast(other);
    }

    public void Reverse()
    {
        LinkListNode temp = _last;

        while(temp != null)
        {
            temp.Revers();
            temp = temp.next;
        }

        (_first, _last) = (_last, _first);
    }

    public bool Contains(T value) => Find(value) != null;
    
    private LinkListNode Find(T value)
    {
        LinkListNode temp = _first;

        while(temp != null)
        {
            if(value.Equals(temp.value))
                return temp;
            temp = temp.next;
        }
        return null;
    }

    public T[] ToArray()
    {
        T[] array = new T[_count];
        int i = 0;
        foreach (var item in this)
            array[i++] = item;
        return array;
    }

    #region IEnumerator<T>, IEnumerable<T>
    public T Current => _current.value;
    object IEnumerator.Current => _current.value;

    public bool MoveNext()
    {
        _current = _current.next;
        return _current != null;
    }
    public void Reset() => _current = new(null, _first);
    public void Dispose() { }
    public IEnumerator<T> GetEnumerator()
    {
        Reset();
        return this;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

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

        public LinkListNode(LinkListNode prev, LinkListNode next)
        {
            value = default;
            this.prev = prev;
            this.next = next;
        }

        public void Revers() => (prev, next) = (next, prev);
    }
}


