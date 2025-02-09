//Assets\Vurbiri\Editor\Types\Collections\IdCollections\Abstract\ADrawerGetConstFieldName.cs
using System;
using System.Reflection;
using UnityEditor;

namespace VurbiriEditor
{
    public abstract class ADrawerGetConstFieldName : PropertyDrawer
    {

        protected string[] GetNames(Type typeField)
        {
            PropertyInfo _names;
            do
            {
                typeField = typeField.BaseType;
                _names = typeField.GetProperty("Names");
            }
            while (_names == null);

            return (string[])_names.GetValue(null);
        }
    }
}
