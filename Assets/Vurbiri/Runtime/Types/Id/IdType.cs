using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vurbiri
{ 
    public abstract class IdType<T> where T : IdType<T>
    {
        public const int None = -1;

        public readonly static int Count;

#if UNITY_EDITOR
        private readonly static IdTypeData _data;

        public static string[] Names_Ed => _data.names;
        public static string[] DisplayNames_Ed => _data.displayNames;
        public static string[] NamesNone_Ed => _data.namesNone;
        public static int[] Values_Ed => _data.values;
        
        static IdType()
        {
            Type typeId = typeof(T), typeInt = typeof(int), typeAttribute = typeof(NotIdAttribute);
            var fields = typeId.GetFields(BindingFlags.Public | BindingFlags.Static);
            IdTypeListData data = new(fields.Length);

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
            IdCacheEd.Add(typeId, Count, _data);
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

#if UNITY_EDITOR
    public static class IdCacheEd
    {
        private static readonly HashSet<Type> _types = new();
        private static readonly Dictionary<Type, IdTypeData> _dates = new();
        private static readonly Dictionary<Type, int> _counts = new();

        static IdCacheEd()
        {
            Type typeId = typeof(IdType<>), baseType;
            object temp;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Vurbiri"))
                {
                    //UnityEngine.Debug.Log(assembly.FullName);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!type.IsGenericType)
                        {
                            baseType = type.BaseType;
                            while (baseType != null && baseType.IsGenericType)
                            {
                                if (baseType.GetGenericTypeDefinition() == typeId)
                                {
                                    temp = baseType.GetField("Count", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                                   
                                    //UnityEngine.Debug.Log(type.Name);
                                    //UnityEngine.Debug.Log(baseType.GetField("Count", BindingFlags.Public | BindingFlags.Static).GetValue(null));
                                    break;
                                }
                                baseType = baseType.BaseType;
                            }
                        }
                    }
                }
            }
        }

        public static IReadOnlyCollection<Type> Types => _types;

        public static bool Contain(Type type) => _types.Contains(type);

        public static int GetCount(Type type) => _counts[type];

        public static string[] GetNames(Type type) => _dates[type].names;
        public static string[] GetDisplayNames(Type type) => _dates[type].displayNames;
        public static int[] GetValues(Type type) => _dates[type].values;

        public static string[] GetNamesNone(Type type) => _dates[type].namesNone;
        public static int[] GetValuesNone(Type type) => _dates[type].valuesNone;

        internal static void Add(Type type, int count, IdTypeData data)
        {
            if (_types.Add(type))
            {
                _counts[type] = count;
                _dates[type] = data;
            }
        }
    }

    internal class IdTypeData
    {
        public readonly string[] names;
        public readonly string[] displayNames;
        public readonly int[] values;

        public readonly string[] namesNone;
        public readonly int[] valuesNone;

        public IdTypeData(IdTypeListData data)
        {
            names = data.names.ToArray();
            displayNames = data.displayNames.ToArray();
            values = data.values.ToArray();

            namesNone = data.namesNone.ToArray();
            valuesNone = data.valuesNone.ToArray();
        }
    }
    internal class IdTypeListData
    {
        public readonly List<string> names;
        public readonly List<string> displayNames;
        public readonly List<int> values;

        public readonly List<string> namesNone;
        public readonly List<int> valuesNone;

        public IdTypeListData(int capacity)
        {
            names = new(capacity);
            displayNames = new(capacity);
            values = new(capacity);

            namesNone = new(capacity + 1) { "None (-1)" };
            valuesNone = new(capacity + 1) { -1 };
        }

        public void Add(string name, int value)
        {
            string displayName = $"{name} ({value})";

            names.Add(name);
            displayNames.Add(displayName);
            values.Add(value);

            namesNone.Add(displayName);
            valuesNone.Add(value);
        }
    }
#endif
}
