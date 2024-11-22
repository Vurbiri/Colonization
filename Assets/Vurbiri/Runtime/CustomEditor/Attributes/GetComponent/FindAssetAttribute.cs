//Assets\Vurbiri\Runtime\CustomEditor\Attributes\GetComponent\FindAssetAttribute.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FindAssetAttribute : PropertyAttribute
    {
        public string fileName;

        public FindAssetAttribute(string fileName = null)
        {
            this.fileName = fileName;
        }
    }
}
