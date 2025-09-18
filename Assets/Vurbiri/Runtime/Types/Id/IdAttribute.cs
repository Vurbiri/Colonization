using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class IdAttribute : PropertyAttribute
    {
        public readonly Type type;

        public IdAttribute(Type idType) => type = idType;
    }
}
