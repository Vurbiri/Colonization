using System;

namespace Vurbiri
{
    public static class TypeExtensions
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
    }
}
