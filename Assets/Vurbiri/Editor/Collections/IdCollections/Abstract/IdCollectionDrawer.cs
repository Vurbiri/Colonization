using System;
using UnityEditor;

namespace VurbiriEditor.Collections
{
    public abstract class IdCollectionDrawer : PropertyDrawer
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
            Type idType = fieldInfo.FieldType;
            if (idType.IsArray) idType = idType.GetElementType();
            idType = idType.GetGenericArguments()[INDEX_TYPE];

            bool isInit = idType == _idType & _names != null;

            if (!isInit && IdTypeCache.Contain(idType))
            {
                _idType = idType;
                _count = IdTypeCache.GetCount(idType);
                _names = IdTypeCache.GetNames(idType);
                isInit = true;
            }
            
            return isInit;
        }
    }
}
