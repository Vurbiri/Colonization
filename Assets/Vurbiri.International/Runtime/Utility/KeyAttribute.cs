using System;
using UnityEngine;

namespace Vurbiri.International
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class KeyAttribute : PropertyAttribute
    {
        public readonly int fileId;

        public KeyAttribute(int fileId)
        {
            this.fileId = fileId;
        }
    }
}
