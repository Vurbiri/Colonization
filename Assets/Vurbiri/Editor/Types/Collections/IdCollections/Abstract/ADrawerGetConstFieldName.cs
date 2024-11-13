using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using Vurbiri;

namespace VurbiriEditor
{
    public abstract class ADrawerGetConstFieldName : PropertyDrawer
    {

        protected List<string> GetNames(Type t_field)
        {
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);
            FieldInfo[] fields = t_field.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length;
            List<string> strings = new(count);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType != t_int || !field.IsLiteral || field.GetCustomAttributes(t_attribute, false).Length > 0 || (int)field.GetValue(null) < 0)
                    continue;

                strings.Add(field.Name);
            }

            return strings;
        }

        protected (string[] names, int[] values) GetNamesAndValues(Type t_field)
        {
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);

            FieldInfo[] fields = t_field.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length;
            List<string> names = new(count);
            List<int> values = new(count);

            FieldInfo field;
            for (int i = 0; i < count; i++)
            {
                field = fields[i];

                if (field.FieldType != t_int | !field.IsLiteral || field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                names.Add(field.Name);
                values.Add((int)field.GetValue(null));
            }

            return (names.ToArray(), values.ToArray());
        }
    }
}
