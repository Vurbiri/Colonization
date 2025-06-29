using System;
using UnityEngine;

namespace Vurbiri
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class InverseAttribute : PropertyAttribute
	{
        public readonly float minF, maxF, offsetF;
        public readonly int minI, maxI, offsetI;

        public InverseAttribute(float min, float max)
        {
            if (min > max) (min, max) = (max, min);

            minF = min;
            maxF = max;
            offsetF = min + max;
        }
        public InverseAttribute(int min, int max)
        {
            if (min > max) (min, max) = (max, min);

            minI = min;
            maxI = max;
            offsetI = min + max;
        }
    }
}
