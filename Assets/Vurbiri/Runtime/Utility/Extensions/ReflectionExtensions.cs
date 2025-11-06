using System;
using System.Reflection;

namespace Vurbiri
{
    public static class ReflectionExtensions
    {
        public static bool Is(this Type self, Type other)
        {
            while (self != null)
            {
                if (self == other)
                    return true;

                self = self.BaseType;
            }

            return false;
        }
        public static bool Is(this Type self, Type other, Type deep)
        {
            while (self != null & self != deep)
            {
                if (self == other)
                    return true;

                self = self.BaseType;
            }

            return false;
        }

        public static Action<T> GetSetor<T>(this object self, string name)
        {
            var property = self.GetType().GetProperty(name);
            return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), self, property.GetSetMethod());
        }
        public static Action<T> GetStaticSetor<T>(this Type self, string name)
        {
            var property = self.GetProperty(name);
            return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), null, property.GetSetMethod());
        }

        public static Func<T> GetGetor<T>(this object self, string name)
        {
            var property = self.GetType().GetProperty(name);
            return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), self, property.GetGetMethod());
        }
        public static Func<T> GetStaticGetor<T>(this Type self, string name)
        {
            var property = self.GetProperty(name);
            return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), null, property.GetGetMethod());
        }

        public static bool Contains(this Assembly assembly, Assembly other) => Contains(assembly, other.GetName());
        public static bool Contains(this Assembly assembly, AssemblyName assemblyName)
        {
            bool contains = assembly.FullName == assemblyName.FullName;
            if (!contains)
            {
                var assemblyNames = assembly.GetReferencedAssemblies();
                for (int i = 0; !contains & i < assemblyNames.Length; i++)
                    contains = assemblyNames[i].FullName == assemblyName.FullName;
            }
            return contains;
        }
    }
}
