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
            Type t_attribute = typeof(HideAllNextIdsAttribute), t_int = typeof(int);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length;
            bool hide = false;
            List<string> strings = new(count);

            foreach (FieldInfo field in fields)
            {
                if (hide || field.GetCustomAttributes(t_attribute, false).Length > 0)
                {
                    hide = true;
                    continue;
                }

                if (field.FieldType != t_int || !field.IsLiteral)
                    continue;

                strings.Add(field.Name);
            }

            return strings;
        }

        protected abstract Type GetTypeId();
    }
}
