using System;
using UnityEditor;
using Vurbiri;

namespace VurbiriEditor.Collections
{
    public abstract class AIdCollectionDrawer : PropertyDrawer
    {
        #region Consts
        protected readonly int INDEX_TYPE = 0, INDEX_VALUE = 1;
        protected readonly string NAME_ARRAY = "_values";
        #endregion

        protected Type _idType;
        protected int _count;
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
            _count = IdTypesCache.GetCount(idType);
            _names = IdTypesCache.GetPositiveNames(idType);
            return isOldId;
        }
    }
}
