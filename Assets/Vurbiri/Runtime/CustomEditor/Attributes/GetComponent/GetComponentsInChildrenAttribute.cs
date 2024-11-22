//Assets\Vurbiri\Runtime\CustomEditor\Attributes\GetComponent\GetComponentsInChildrenAttribute.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GetComponentsInChildrenAttribute : PropertyAttribute
    {
        public int indexGeneric;
        public bool includeInactive;

        public GetComponentsInChildrenAttribute(int indexGeneric = 0, bool includeInactive = false)
        {
            this.includeInactive = includeInactive;
            this.indexGeneric = indexGeneric;
        }
    }
}
