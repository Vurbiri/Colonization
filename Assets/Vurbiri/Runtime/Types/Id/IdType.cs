//Assets\Vurbiri\Runtime\Types\Id\AIdType.cs
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
        private readonly static List<string> _names;
        private readonly static List<string> _positiveNames;
        private readonly static List<string> _displayNames;
        private readonly static List<int> _values;

        public static string[] Names => _names.ToArray();
        public static string[] PositiveNames => _positiveNames.ToArray();
        public static string[] DisplayNames => _displayNames.ToArray();
        public static int[] Values => _values.ToArray();

        public static string GetName(Id<T> id) => _names[id.Value - Min];

        static IdType()
        {
            Type typeId = typeof(T), typeInt = typeof(int), typeAttribute = typeof(NotIdAttribute);
            FieldInfo[] fields = typeId.GetFields(BindingFlags.Public | BindingFlags.Static);

            if (fields.Length == 0)
                Debug.LogError($"Нет public static полей. Класс: {typeId.Name}");

            _names = new(fields.Length);
            _positiveNames = new(fields.Length);
            _displayNames = new(fields.Length);
            _values = new(fields.Length);

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
                {
                    Count++;
                    _positiveNames.Add(field.Name);
                }

                oldValue = value;
                _names.Add(field.Name);
                _displayNames.Add($"{field.Name} ({value})");
                _values.Add(value);
            }

            if (Count == 0)
                Debug.LogError($"Не найдено public const int полей. Класс: {typeId.Name}");

            //Message.Log($"Create {typeId.Name}. min: {Min}, count: {Count}");
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
        protected static void RunConstructor() { }
    }
}
