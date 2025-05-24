using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct FloatRnd
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public readonly float Min => _min;
        public readonly float Max => _max;

        public readonly float Roll => Random.Range(_min, _max);
        public readonly float RollMoreAvg => Random.Range((_min + _max) * 0.5f, _max);
        public readonly float RollLessAvg => Random.Range(_min, (_min + _max) * 0.5f);
        public readonly float Avg => (_min + _max) * 0.5f;

        public FloatRnd(float min, float max)
        {
            if (min > max)
            {
                _min = max;
                _max = min;
                return;
            }

            _min = min;
            _max = max;
        }

        public FloatRnd(FloatRnd value, float ratio)
        {
            _min = value._min * ratio;
            _max = value._max * ratio;
        }

        public FloatRnd(Vector2 vector) : this(vector.x, vector.y) { }

        public static implicit operator float(FloatRnd mm) => Random.Range(mm._min, mm._max);

        public static FloatRnd operator *(FloatRnd mm1, FloatRnd mm2) => new(mm1._min * mm2._min, mm1._max * mm2._max);
        public static FloatRnd operator +(FloatRnd mm1, FloatRnd mm2) => new(mm1._min + mm2._min, mm1._max + mm2._max);
        public static FloatRnd operator -(FloatRnd mm1, FloatRnd mm2) => new(mm1._min - mm2._min, mm1._max - mm2._max);


        public static float operator *(float value, FloatRnd mm) => value * Random.Range(mm._min, mm._max);
        public static float operator *(FloatRnd mm, float value) => value * Random.Range(mm._min, mm._max);
        public static Vector3 operator *(Vector3 value, FloatRnd mm) => value * Random.Range(mm._min, mm._max);
        public static Vector3 operator *(FloatRnd mm, Vector3 value) => value * Random.Range(mm._min, mm._max);
        public static Vector2 operator *(Vector2 value, FloatRnd mm) => value * Random.Range(mm._min, mm._max);
        public static Vector2 operator *(FloatRnd mm, Vector2 value) => value * Random.Range(mm._min, mm._max);

        public static float operator +(float value, FloatRnd mm) => value + Random.Range(mm._min, mm._max);
        public static float operator +(FloatRnd mm, float value) => value + Random.Range(mm._min, mm._max);

        public static float operator -(float value, FloatRnd mm) => value - Random.Range(mm._min, mm._max);
        public static float operator -(FloatRnd mm, float value) => Random.Range(mm._min, mm._max) - value;

        public override readonly string ToString() => $"(min: {_min}, max: {_max})";
    }
}
