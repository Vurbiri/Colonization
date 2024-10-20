using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AIdType<T> where T : AIdType<T>
    {
        private readonly static int _min;
        private readonly static int _countAll;
        protected static int _count;

        public static int Min => _min;
        public static int Count => _count;
        public static int CountAll => _countAll;

#if UNITY_EDITOR
        private readonly static List<string> _names;
        public static IReadOnlyList<string> Names => _names;

        static AIdType() 
        {
            Type t_child = typeof(T), t_int = typeof(int), t_attribute = typeof(NotIdAttribute);
            FieldInfo[] fields = t_child.GetFields(BindingFlags.Public | BindingFlags.Static);

            if (fields.Length == 0)
                Debug.LogError($"Нет public static полей. Класс: {t_child.Name}");

            _names = new(fields.Length);

            _count = 0; _min = int.MaxValue;
            int value;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                if (field.FieldType != t_int || !field.IsLiteral)
                    Debug.LogError($"Поле {t_child.Name}.{field.Name} должно иметь тип int и быть константным.");

                value = (int)field.GetValue(null);
                _min = Mathf.Min(value, _min);

                if (value >= 0 && value != _count++)
                    Debug.LogError($"Неожидаемое значение поля {t_child.Name}.{field.Name} = {value} вместо {_count - 1}");

                _names.Add(field.Name);
            }

            if (_count == 0)
                Debug.LogError($"Нет положительных public const полей. Класс: {t_child.Name}");

            _countAll = _count - _min;

            //Message.Log($"Create {t_child.Name}. min: {_min}, count: {_count}, countAll: {_countAll}");
        }

        public static bool IsValidate(int value) => value >= _min && value < _count;
#else
        static AIdType()
        {
            Type t_child = typeof(T), t_attribute = typeof(NotIdAttribute);
            FieldInfo[] fields = t_child.GetFields(BindingFlags.Public | BindingFlags.Static);

            _count = 0; _min = int.MaxValue;
            int value;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                value = (int)field.GetValue(null);
                _min = Mathf.Min(value, _min);
                if (value >= 0)
                    _count++;
            }
            _countAll = _count - _min;

            Message.Log($"Create {t_child.Name}. min: {_min}, count: {_count}, countAll: {_countAll}");
        }
#endif

        protected static void RunConstructor() { }
    }
}
