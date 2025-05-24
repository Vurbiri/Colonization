using System;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MaxAttribute : Attribute
    {
        public readonly float max;

        public MaxAttribute(float max)
        {
            this.max = max;
        }

        public MaxAttribute(int max)
        {
            this.max = max;
        }
    }
}
