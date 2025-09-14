using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class AnimatorParameterAttribute : PropertyAttribute { }
}
