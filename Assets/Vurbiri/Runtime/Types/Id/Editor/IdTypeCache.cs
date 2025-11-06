using System;
using System.Collections.Generic;
using System.Reflection;
using Vurbiri;

namespace VurbiriEditor
{
    public static class IdTypeCache
    {
        private static readonly HashSet<Type> _types = new();
        private static readonly Dictionary<Type, IdTypeData> _dates = new();
        private static readonly Dictionary<Type, int> _counts = new();

        static IdTypeCache()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            Type typeId = typeof(IdType<>), baseType;
            object temp;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.Contains(assemblyName))
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!type.IsGenericType && type.IsAbstract)
                        {
                            baseType = type.BaseType;
                            while (baseType != null && baseType.IsGenericType)
                            {
                                if (baseType.GetGenericTypeDefinition() == typeId)
                                {
                                    var field = baseType.GetField("Count", BindingFlags.Public | BindingFlags.Static);
                                    if (field != null)
                                    {
                                        temp = field.GetValue(null);
                                        break;
                                    }
                                }
                                baseType = baseType.BaseType;
                            }
                        }
                    }
                }
            }
        }

        public static HashSet<Type> Types => _types;

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
}
