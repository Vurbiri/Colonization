using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vurbiri
{
    public abstract class AIdType<T> where T : AIdType<T>
    {
        private static readonly int _count;
        public static int Count => _count;
        private static readonly int _realCount;


#if UNITY_EDITOR
        public static IReadOnlyList<string> Names => _names;
        private static readonly List<string> _names;
#endif

        static AIdType()
        {
            int count = 0, realCount = 0;
            bool hide = false;
            Type t_child = typeof(T), t_attribute = typeof(HideAllNextIdsAttribute);
            FieldInfo[] fields = t_child.GetFields(BindingFlags.Public | BindingFlags.Static);

#if UNITY_EDITOR
            if (fields.Length == 0)
                throw new Exception($"Нет public static полей. Класс: {t_child.Name}");

            Type t_int = typeof(int);
            _names = new(fields.Length);
#endif
            
            foreach (FieldInfo field in fields)
            {

                if (hide || field.GetCustomAttributes(t_attribute, false).Length > 0)
                {
#if UNITY_EDITOR
                    if ((int)field.GetValue(null) != count + realCount)
                        throw new Exception($"Неожидаемое значение скрытого поля {t_child.Name}.{field.Name} = {field.GetValue(null)} вместо {count + realCount}");

                    _names.Add(field.Name);
#endif
                    hide = true;
                    realCount++;
                    continue;
                }
#if UNITY_EDITOR
                if (field.FieldType != t_int || !field.IsLiteral)
                    throw new Exception($"Поле {t_child.Name}.{field.Name} должно иметь тип int и быть константным.");

                if ((int)field.GetValue(null) != count++)
                    throw new Exception($"Неожидаемое значение поля {t_child.Name}.{field.Name} = {field.GetValue(null)} вместо {count - 1}");

                _names.Add(field.Name);
#else
                count++;
#endif
            }

            if ((_count = count) == 0)
                throw new Exception($"Нет положительных public const полей. Класс: {t_child.Name}");

            _realCount = count + realCount;

#if !UNITY_EDITOR
            Message.Log($"Create {t_child.Name}: {_count} / {_realCount}");
#endif
        }

        protected static void RunConstructor() { }

        public static bool IsValidate(int value) => value >= 0 && value < _realCount;

    }
}
