using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class IdAttribute : PropertyAttribute
    {
        public readonly Type type;

        public IdAttribute(Type idType)
        {
            if (idType != null && IdTypesCacheEditor.Contain(idType))
                type = idType;
            else
                Errors.Argument("idType", idType);
        }
	}
}
