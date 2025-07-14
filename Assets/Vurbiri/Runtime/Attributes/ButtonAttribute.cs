using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string methodName;
        public readonly string caption;

        public ButtonAttribute(string methodName)
        {
            this.methodName = methodName;
            this.caption = methodName;
        }
        public ButtonAttribute(string methodName, string caption)
        {
            this.methodName = methodName;
            this.caption = caption;
        }
    }
}
