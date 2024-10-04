using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vurbiri
{
    public abstract class AIdType<T> where T : AIdType<T>
    {
        private static readonly int _count;
        public static int Count => _count;

#if UNITY_EDITOR
        public static List<string> Names => _names;
        private static readonly List<string> _names;
#endif

        static AIdType()
        {
            Type t_child = typeof(T), t_int = typeof(int), t_attribute = typeof(NotIdAttribute);
            FieldInfo[] fields = t_child.GetFields(BindingFlags.Public | BindingFlags.Static);

#if UNITY_EDITOR
            _names = new(fields.Length);
#endif
            int value = 0;
            foreach (FieldInfo field in fields)
            {
                if(field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                if (field.FieldType != t_int || !field.IsLiteral)
                    throw new Exception($"Поле {field.Name} класса {t_child.Name} должно иметь тип int и быть константным.");

                if ((int)field.GetValue(null) != value++)
                    throw new Exception($"Неожидаемое значение поля {field.Name} = {field.GetValue(null)} вместо {value - 1} класса {t_child.Name}");

#if UNITY_EDITOR
                _names.Add(field.Name);
#endif
            }

            if ((_count = value) == 0)
                throw new Exception($"Нет public const полей. Класс: {t_child.Name}");
        }

        protected static void RunConstructor() { }

        public static bool IsValidate(int value) => value >= 0 && value < _count;

    }
}
