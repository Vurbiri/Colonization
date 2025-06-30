using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Road
    {
        private class Points
        {
            private const float OFFSET_Y = 0.0125f;
            private const int BASE_CAPACITY = 31;

            private readonly LineRenderer _roadRenderer;
            private readonly Road _road;

            private Vector3[] _values = new Vector3[BASE_CAPACITY];
            private int _capacity = BASE_CAPACITY;
            private int _count;

            public Vector3 this[int index] => _values[index];

            public int Count => _count;
            public Vector3 Start => _values[0];
            public Vector3 End => _values[_count];

            public Points(Road road, LineRenderer roadRenderer, Vector3 start, Vector3 end)
            {
                _roadRenderer = roadRenderer;
                _road = road;

                start.y = OFFSET_Y;
                _values[0] = start; _count = 1;

                Add(start, end);
            }

            public void Add(Vector3 start, Vector3 end)
            {
                int addCount = _road._rangeCount, nextCount = _count + addCount;

                if (nextCount > _capacity) 
                    GrowArray(nextCount);

                start.y = end.y = OFFSET_Y;
                Vector3 step = (end - start) / addCount, offsetSide = Vector3.Cross(Vector3.up, step.normalized);
                float sign = Chance.Select(1f, -1f), signStep = -1f;

                nextCount--;
                for (int i = _count; i < nextCount; i++)
                {
                    sign *= signStep;
                    _values[i] = start += _road._lengthFluctuation * step + _road._rateWave * sign * offsetSide;
                }
                _values[nextCount] = end;
                _count = nextCount + 1;

                SetPositions();
            }

            public void Insert(Vector3 start, Vector3 end)
            {
                int addCount = _road._rangeCount, nextCount = _count + addCount;

                if (nextCount > _capacity)
                    GrowArray(nextCount, addCount);
                else
                    Shift(nextCount);

                start.y = end.y = OFFSET_Y;
                Vector3 step = (end - start) / addCount, offsetSide = Vector3.Cross(Vector3.up, step.normalized);
                float sign = Chance.Select(1f, -1f), signStep = -1f;

                for (int i = addCount - 1; i > 0; i--)
                {
                    sign *= signStep;
                    _values[i] = start += _road._lengthFluctuation * step + _road._rateWave * sign * offsetSide;
                }
                _values[0] = end;
                _count = nextCount;

                SetPositions();
            }

            public void AddRange(Points other)
            {
                int addCount = other._count - 1;
                int totalCount = _count + addCount;
                if (totalCount > _capacity)
                    GrowArray(totalCount);

                for (int i = _count, j = 1; j <= addCount; i++, j++)
                    _values[i] = other._values[j];
                _count = totalCount;

                SetPositions();
            }

            public void AddReverseRange(Points other)
            {
                int addCount = other._count - 1;
                int totalCount = _count + addCount;
                if (totalCount > _capacity)
                    GrowArray(totalCount);

                for (int i = _count, j = addCount - 1; j >= 0; i++, j--)
                    _values[i] = other._values[j];
                _count = totalCount;

                SetPositions();
            }

            public void InsertRange(Points other)
            {
                int addCount = other._count - 1;
                int totalCount = _count + addCount;
                if (totalCount > _capacity)
                    GrowArray(totalCount, addCount);
                else
                    Shift(totalCount);

                for (int i = addCount - 1; i >= 0; i--)
                    _values[i] = other._values[i];
                _count = totalCount;

                SetPositions();
            }

            public void InsertReverseRange(Points other)
            {
                int addCount = other._count - 1;
                int totalCount = _count + addCount;
                if (totalCount > _capacity)
                    GrowArray(totalCount, addCount);
                else
                    Shift(totalCount);

                for (int i = 0, j = addCount; i < addCount; i++, j--)
                    _values[i] = other._values[j];
                _count = totalCount;

                SetPositions();
            }

            public bool Remove(Vector3 point)
            {
                point.y = OFFSET_Y;

                int i = _count;
                while (i --> 0)
                {
                    if (_values[i] == point)
                    {
                        _count = i + 1;
                        SetPositions();
                        return true;
                    }
                }
                return false;
            }

            public bool Extract(Vector3 point)
            {
                point.y = OFFSET_Y;

                for (int i = 0; i < _count; i++)
                {
                    if (_values[i] == point)
                    {
                        _count -= i;
                        for (int j = 0; j < _count; j++, i++)
                            _values[j] = _values[i];

                        SetPositions();
                        return true;
                    }
                }
                return false;
            }

            private void SetPositions()
            {
                _roadRenderer.positionCount = _count;
                _roadRenderer.SetPositions(_values);
            }

            private void Shift(int count)
            {
                for (int i = count - 1, j = _count - 1; j >= 0; i--, j--)
                    _values[i] = _values[j];
            }

            private void GrowArray(int count, int offset = 0)
            {
                while (_capacity < count)
                    _capacity = _capacity << 1 | BASE_CAPACITY;

                Vector3[] array = new Vector3[_capacity];
                for (int i = 0, j = offset; i < _count; i++, j++)
                    array[j] = _values[i];

                _values = array;
            }
        }
    }
}
