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
        public static IReadOnlyList<string> Names => _names;
        private static readonly List<string> _names;
#endif

        static AIdType()
        {
            int count = 0;
            Type t_child = typeof(T), t_attribute = typeof(NotIdAttribute);
            FieldInfo[] fields = t_child.GetFields(BindingFlags.Public | BindingFlags.Static);

#if UNITY_EDITOR
            Type t_int = typeof(int);
            _names = new(fields.Length);
#endif
            
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;
#if UNITY_EDITOR
                if (field.FieldType != t_int || !field.IsLiteral)
                    throw new Exception($"Поле {field.Name} класса {t_child.Name} должно иметь тип int и быть константным.");

                if ((int)field.GetValue(null) != count++)
                    throw new Exception($"Неожидаемое значение поля {field.Name} = {field.GetValue(null)} вместо {count - 1} класса {t_child.Name}");


                _names.Add(field.Name);
#else
                count++;
#endif
            }

            if ((_count = count) == 0)
                throw new Exception($"Нет public const полей. Класс: {t_child.Name}");

#if !UNITY_EDITOR
            Message.Log($"Create {t_child.Name}");
#endif
        }

        protected static void RunConstructor() { }

        public static bool IsValidate(int value) => value >= 0 && value < _count;

    }
}
