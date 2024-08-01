using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FindObjectAttribute : PropertyAttribute
    {
        public bool includeInactive;

        public FindObjectAttribute(bool includeInactive = false) => this.includeInactive = includeInactive;
    }
}