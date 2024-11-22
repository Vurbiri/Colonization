//Assets\Vurbiri\Runtime\CustomEditor\Attributes\RenameAttribute.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RenameAttribute : PropertyAttribute
    {
        public string Name { get; private set; }

        public RenameAttribute(string name) => Name = name;
    }
}
