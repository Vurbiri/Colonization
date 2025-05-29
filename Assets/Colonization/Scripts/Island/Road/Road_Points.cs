using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Road
    {
        private class Points
        {
            private const float OFFSET_Y = 0.0125f;
            private const int BASE_CAPACITY = 32;

            private readonly LineRenderer _roadRenderer;

            private FloatRnd _rateWave;
            private IntRnd _rangeCount = new(3, 6);
            private FloatRnd _lengthFluctuation = new(0.85f, 1.15f);

            private Vector3[] _values = new Vector3[BASE_CAPACITY];
            private int _capacity = BASE_CAPACITY;
            private int _count;

            public Points(LineRenderer roadRenderer, FloatRnd rateWave, Vector3 start, Vector3 end)
            {
                _roadRenderer = roadRenderer;
                _rateWave = rateWave;

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
                    _capacity = _capacity << 1 | BASE_CAPACITY;

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
    }
}
