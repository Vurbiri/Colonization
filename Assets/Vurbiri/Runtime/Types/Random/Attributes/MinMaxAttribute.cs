//Assets\Vurbiri\Runtime\Types\Random\Attributes\MinMaxAttribute.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MinMaxAttribute : PropertyAttribute
    {
        public readonly float min;
        public readonly float max;

        public MinMaxAttribute(float min, float max)
        {
            if (min > max)
                (min, max) = (max, min);

            this.min = min;
            this.max = max;
        }

        public MinMaxAttribute(int min, int max)
        {
            if (min > max)
                (min, max) = (max, min);

            this.min = min;
            this.max = max;
        }
    }
}
