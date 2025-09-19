using Newtonsoft.Json;
using UnityEngine;

namespace Vurbiri
{
    [System.Serializable, JsonConverter(typeof(Converter))]
    public struct IntRnd
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        public readonly int Min => _min;
        public readonly int Max => _max - 1;

        public readonly int Roll => Random.Range(_min, _max);
        public readonly int RollMoreAvg => Random.Range((_min + _max) >> 1, _max);
        public readonly int Avg => (_min + _max) >> 1;

        public IntRnd(int min, int max)
        {
            if (min > max)
            {
                _min = max;
                _max = min + 1;
                return;
            }

            _min = min;
            _max = max + 1;
        }

        public IntRnd(Vector2Int vector) : this(vector.x, vector.y) { }

        public static implicit operator int(IntRnd mm) => Random.Range(mm._min, mm._max);
        public static explicit operator byte(IntRnd mm) => (byte)Random.Range(mm._min, mm._max);

        public static IntRnd operator *(IntRnd mm1, IntRnd mm2) => new(mm1._min * mm2._min, (mm1._max - 1) * (mm2._max - 1));
        public static IntRnd operator +(IntRnd mm1, IntRnd mm2) => new(mm1._min + mm2._min, mm1._max + mm2._max - 2);
        public static IntRnd operator -(IntRnd mm1, IntRnd mm2) => new(mm1._min - mm2._min, mm1._max - mm2._max);
                
        public static int operator *(int value, IntRnd mm) => value * Random.Range(mm._min, mm._max);
        public static int operator *(IntRnd mm, int value) => value * Random.Range(mm._min, mm._max);
        public static Vector3Int operator *(Vector3Int value, IntRnd mm) => value * Random.Range(mm._min, mm._max);
        public static Vector3Int operator *(IntRnd mm, Vector3Int value) => value * Random.Range(mm._min, mm._max);
        public static Vector2Int operator *(Vector2Int value, IntRnd mm) => value * Random.Range(mm._min, mm._max);
        public static Vector2Int operator *(IntRnd mm, Vector2Int value) => value * Random.Range(mm._min, mm._max);
        
        public static int operator +(int value, IntRnd mm) => value + Random.Range(mm._min, mm._max);
        public static int operator +(IntRnd mm, int value) => value + Random.Range(mm._min, mm._max);

        public static int operator -(int value, IntRnd mm) => value - Random.Range(mm._min, mm._max);
        public static int operator -(IntRnd mm, int value) => Random.Range(mm._min, mm._max) - value;

        public override readonly string ToString() => $"(min: {_min}, max: {_max - 1})";

        #region Nested Json Converter
        //***************************************************************************
        sealed public class Converter : AJsonConverter<IntRnd>
        {
            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                var data = serializer.Deserialize<int[]>(reader);
                var value = new IntRnd
                {
                    _min = data[0],
                    _max = data[1]
                };

                return value;
            }

            protected override void WriteJson(JsonWriter writer, IntRnd value, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                writer.WriteValue(value._min);
                writer.WriteValue(value._max);
                writer.WriteEndArray();
            }
        }
        #endregion
    }
}
