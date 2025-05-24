using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MinMaxAttribute : PropertyAttribute
    {
        public readonly float min;
        public readonly float max;

        public readonly string nameMin = "_min";
        public readonly string nameMax = "_max";

        public MinMaxAttribute(float min, float max)
        {
            if (min > max) (min, max) = (max, min);

            this.min = min;
            this.max = max;
        }

        public MinMaxAttribute(string nameMin, float min, string nameMax, float max) : this(min, max)
        {
            this.nameMin = nameMin;
            this.nameMax = nameMax;
        }
    }
}
