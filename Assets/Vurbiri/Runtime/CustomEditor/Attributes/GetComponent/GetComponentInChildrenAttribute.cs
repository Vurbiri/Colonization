//Assets\Vurbiri\Runtime\CustomEditor\Attributes\GetComponent\GetComponentInChildrenAttribute.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GetComponentInChildrenAttribute : PropertyAttribute
    {
        public string name;
        public bool includeInactive;

        public GetComponentInChildrenAttribute(string name = null, bool includeInactive = false)
        {
            this.name = name;
            this.includeInactive = includeInactive;
        }
    }
}
