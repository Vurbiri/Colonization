//Assets\Vurbiri\Runtime\CustomEditor\Attributes\GetComponent\GetComponentInParentAttribute.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GetComponentInParentAttribute : PropertyAttribute
    {

    }
}
