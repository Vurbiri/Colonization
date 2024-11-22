//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\Abstract\AIdType.cs
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AIdType<T> where T : AIdType<T>
    {
        private readonly static int _min;
        private readonly static int _count;

        public static int Min => _min;
        public static int Count => _count;

#if UNITY_EDITOR
        private readonly static List<string> _names;
        public static string GetName(Id<T> id) => _names[id.Value - _min];

        static AIdType() 
        {
            Type t_child = typeof(T), t_int = typeof(int), t_attribute = typeof(NotIdAttribute);
            FieldInfo[] fields = t_child.GetFields(BindingFlags.Public | BindingFlags.Static);

            if (fields.Length == 0)
                Debug.LogError($"��� public static �����. �����: {t_child.Name}");

            _names = new(fields.Length);

            _count = 0;
            int value;
            int? oldValue = null;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                if (field.FieldType != t_int | !field.IsLiteral)
                    Debug.LogError($"���� {t_child.Name}.{field.Name} ������ ����� ��� int � ���� �����������.");

                value = (int)field.GetValue(null);

                if (oldValue == null)
                {
                    _min = value;
                }
                else if (value != oldValue + 1)
                {
                    Debug.LogError($"����������� �������� ���� {t_child.Name}.{field.Name} = {value} ������ {oldValue + 1}");
                }

                if (value >= 0)
                    _count++;

                oldValue = value;
                _names.Add(field.Name);
            }

            if (_count == 0)
                Debug.LogError($"��� ������������� public const �����. �����: {t_child.Name}");

            //Message.Log($"Create {t_child.Name}. min: {_min}, count: {_count}, countAll: {_countAll}");
        }

        
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
        }
#endif
        public static bool IsValidate(int value) => value >= _min & value < _count;

        protected static void RunConstructor() { }
    }
}
