using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using Vurbiri;

namespace VurbiriEditor
{
    public abstract class ADrawerGetConstFieldName : PropertyDrawer
    {
        protected List<string> GetNames()
        {
            Type type = GetTypeId();
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

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

        protected abstract Type GetTypeId();
    }
}
