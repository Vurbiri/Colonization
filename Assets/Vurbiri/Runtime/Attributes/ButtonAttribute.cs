using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string methodName;
        public readonly string caption;
        public readonly bool drawProperty;

        public ButtonAttribute(string methodName, bool drawProperty = true)
        {
            this.methodName = methodName;
            this.caption = methodName;
            this.drawProperty = drawProperty;
        }
        public ButtonAttribute(string methodName, string caption, bool drawProperty = true)
        {
            this.methodName = methodName;
            this.caption = caption;
            this.drawProperty = drawProperty;
        }
    }
}
