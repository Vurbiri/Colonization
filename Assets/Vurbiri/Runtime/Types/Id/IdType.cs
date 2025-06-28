using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Vurbiri
{
    public abstract class IdType<T> where T : IdType<T>
    {
        public readonly static int Min;
        public readonly static int Count;

#if UNITY_EDITOR
        private readonly static IdTypeData _data;

        public static string[] Names => _data.names.ToArray();
        public static string[] PositiveNames => _data.positiveNames.ToArray();
        public static string[] DisplayNames => _data.displayNames.ToArray();
        public static int[] Values => _data.values.ToArray();

        public static string GetName(Id<T> id) => _data.names[id.Value - Min];

        static IdType()
        {
            Type typeId = typeof(T), typeInt = typeof(int), typeAttribute = typeof(NotIdAttribute);
            FieldInfo[] fields = typeId.GetFields(BindingFlags.Public | BindingFlags.Static);

            if (fields.Length == 0)
                Debug.LogError($"Нет public static полей. Класс: {typeId.Name}");

            _data = new(fields.Length);

            Count = 0;
            int value;
            int? oldValue = null;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttributes(typeAttribute, false).Length > 0)
                    continue;

                if (field.FieldType != typeInt | !field.IsLiteral)
                    Debug.LogError($"Поле {typeId.Name}.{field.Name} не int или не const.");

                value = (int)field.GetValue(null);

                if (oldValue == null)
                {
                    Min = value;
                }
                else if (value != oldValue + 1)
                {
                    Debug.LogError($"Неверное значение поля: {typeId.Name}.{field.Name} = {value} должно быть {oldValue + 1}");
                }

                if (value >= 0)
                    Count++;

                oldValue = value;
                _data.Add(field.Name, value);
            }

            if (Count == 0)
                Debug.LogError($"Не найдено public const int полей. Класс: {typeId.Name}");

            IdTypesCache.Add(typeId, Count, Min, _data);
        }
#else
        static IdType()
        {
            Type typeId = typeof(T), typeInt = typeof(int), typeAttribute = typeof(NotIdAttribute);
            FieldInfo[] fields = typeId.GetFields(BindingFlags.Public | BindingFlags.Static);

            Count = 0; Min = int.MaxValue;
            int value;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType != typeInt | !field.IsLiteral || field.GetCustomAttributes(typeAttribute, false).Length > 0)
                    continue;

                value = (int)field.GetValue(null);
                Min = Mathf.Min(value, Min);
                if (value >= 0)
                    Count++;
            }
        }
#endif
        protected static void ConstructorRun() { }
    }

#if UNITY_EDITOR
    public static class IdTypesCache
    {
        private static readonly HashSet<Type> _types = new();
        private static readonly Dictionary<Type, IdTypeData> _dates = new();
        private static readonly Dictionary<Type, int> _counts = new();
        private static readonly Dictionary<Type, int> _mins = new();

        public static IReadOnlyCollection<Type> Types => _types;

        public static bool Contain(Type type) => _types.Contains(type);

        public static int GetCount(Type type) => _counts[type];
        public static int GetMin(Type type) => _mins[type];

        public static string[] GetNames(Type type) => _dates[type].names.ToArray();
        public static string[] GetPositiveNames(Type type) => _dates[type].positiveNames.ToArray();
        public static string[] GetDisplayNames(Type type) => _dates[type].displayNames.ToArray();
        public static int[] GetValues(Type type) => _dates[type].values.ToArray();

        internal static void Add(Type type, int count, int min, IdTypeData data)
        {
            if (_types.Add(type))
            {
                _counts[type] = count;
                _mins[type] = min;
                _dates[type] = data;
            }
        }
    }

    internal class IdTypeData
    {
        public readonly List<string> names;
        public readonly List<string> positiveNames;
        public readonly List<string> displayNames;
        public readonly List<int> values;

        public IdTypeData(int capacity)
        {
            names = new(capacity);
            positiveNames = new(capacity);
            displayNames = new(capacity);
            values = new(capacity);
        }

        public void Add(string name, int value)
        {
            names.Add(name);
            if (value >= 0) positiveNames.Add(name);
            displayNames.Add($"{name} ({value})");
            values.Add(value);
        }
    }
#endif
}
