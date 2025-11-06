using System;
using System.Reflection;

namespace Vurbiri
{ 
    public abstract class IdType<T> where T : IdType<T>
    {
        public const int None = -1;

        public readonly static int Count;

#if UNITY_EDITOR
        private readonly static VurbiriEditor.IdTypeData _data;

        public static string[] Names_Ed => _data.names;
        public static string[] DisplayNames_Ed => _data.displayNames;
        public static string[] NamesNone_Ed => _data.namesNone;
        public static int[] Values_Ed => _data.values;
        
        static IdType()
        {
            Type typeId = typeof(T), typeInt = typeof(int), typeAttribute = typeof(NotIdAttribute);
            var fields = typeId.GetFields(BindingFlags.Public | BindingFlags.Static);
            VurbiriEditor.IdTypeListData data = new(fields.Length);

            Count = 0;
            int value, oldValue = None;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttributes(typeAttribute, false).Length > 0)
                    continue;

                if (field.FieldType != typeInt | !field.IsLiteral)
                    UnityEngine.Debug.LogError($"<b>[{typeId.Name}]</b> The field <b>{field.Name}</b> not <b>int</b> or not <b>const</b>.");

                value = (int)field.GetValue(null);
                if (value != oldValue + 1)
                    UnityEngine.Debug.LogError($"<b>[{typeId.Name}]</b> Invalid field value: <b>{field.Name} = {value}</b> should be <b>{oldValue + 1}</b>.");

                Count++;
                data.Add(field.Name, value);
                oldValue = value;
            }

            if (Count == 0)
                UnityEngine.Debug.LogError($"<b>[{typeId.Name}]</b> No <b>public const int</b> fields found.");

            _data = new(data);
            VurbiriEditor.IdTypeCache.Add(typeId, Count, _data);
        }
#else
        static IdType()
        {
            Type typeId = typeof(T), typeInt = typeof(int), typeAttribute = typeof(NotIdAttribute);
            var fields = typeId.GetFields(BindingFlags.Public | BindingFlags.Static);

            Count = 0;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeInt & field.IsLiteral && field.GetCustomAttributes(typeAttribute, false).Length == 0)
                    Count++;
            }
        }
#endif
    }
}
