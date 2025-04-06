//Assets\Vurbiri\Editor\Types\Collections\IdCollections\Abstract\ADrawerGetConstFieldName.cs
using System;
using System.Reflection;
using UnityEditor;

namespace VurbiriEditor.Collections
{
    public abstract class AIdCollectionDrawer : PropertyDrawer
    {
        #region Consts
        private const string TP_NAMES = "PositiveNames";
        protected const int INDEX_TYPE = 0, INDEX_VALUE = 1;
        protected const string NAME_ARRAY = "_values";
        #endregion

        private Type _type;
        protected string[] _names;
        protected readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        protected readonly float _ySpace = EditorGUIUtility.standardVerticalSpacing;

        protected bool GetPositiveNames()
        {
            Type typeId = fieldInfo.FieldType.GetGenericArguments()[INDEX_TYPE];

            if (typeId == _type & _names != null) return true;

            _type = typeId;

            typeId = typeId.BaseType;
            PropertyInfo namesProperty = null;
            while (typeId != null & namesProperty == null)
            {
                namesProperty = typeId.GetProperty(TP_NAMES);
                typeId = typeId.BaseType;
            }

            _names = (string[])namesProperty.GetValue(null);
            return _names != null;
        }
    }
}
