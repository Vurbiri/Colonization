using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class AnimParamsAttribute : PropertyAttribute
    {
        public readonly string[] names;
        public readonly int[] values;

        public AnimParamsAttribute(params string[] paramNames)
        {
            int count = paramNames.Length;
            names = paramNames;
            values = new int[count];
            for (int i = 0; i < count; i++)
                values[i] = Animator.StringToHash(paramNames[i]);
        }
    }
}
