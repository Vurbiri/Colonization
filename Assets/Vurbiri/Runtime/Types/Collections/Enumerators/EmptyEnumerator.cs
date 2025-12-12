using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public class EmptyEnumerator<T> : IEnumerator<T>
    {
        private static readonly IEnumerator<T> s_instance = new EmptyEnumerator<T>();

        public static IEnumerator<T> Instance { [Impl(256)] get => s_instance; }

        public T Current => default;
        object IEnumerator.Current => default;

        private EmptyEnumerator() { }

        public bool MoveNext() => false;

        public void Reset() { }
        public void Dispose() { }
    }
}
