using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string methodName;

        public ButtonAttribute(string methodName) => this.methodName = methodName;
            
    }
}
