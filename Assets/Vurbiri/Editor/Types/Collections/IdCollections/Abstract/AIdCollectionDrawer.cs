using System;
using System.Reflection;
using UnityEditor;

namespace VurbiriEditor.Collections
{
    public abstract class AIdCollectionDrawer : PropertyDrawer
    {
        #region Consts
        private readonly string TP_NAMES = "PositiveNames";
        protected readonly int INDEX_TYPE = 0, INDEX_VALUE = 1;
        protected readonly string NAME_ARRAY = "_values";
        #endregion

        protected Type _idType;
        protected string[] _names;
        protected readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        protected readonly float _ySpace = EditorGUIUtility.standardVerticalSpacing;

        protected bool SetPositiveNames()
        {
            Type idType = fieldInfo.FieldType.GetGenericArguments()[INDEX_TYPE];
            bool isOldId = idType == _idType;

            if (isOldId & _names != null) 
                return true;

            _idType = idType;

            idType = idType.BaseType;
            PropertyInfo namesProperty = null;
            while (idType != null & namesProperty == null)
            {
                namesProperty = idType.GetProperty(TP_NAMES);
                idType = idType.BaseType;
            }

            _names = (string[])namesProperty.GetValue(null);
            return isOldId;
        }
    }
}
