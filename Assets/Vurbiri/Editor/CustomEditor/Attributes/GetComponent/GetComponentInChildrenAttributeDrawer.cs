using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(GetComponentInChildrenAttribute))]
    internal class GetComponentInChildrenAttributeDrawer : AGetComponentAttributeDrawer
    {
        protected override void SetProperty(SerializedProperty property, GameObject gameObject, System.Type type)
        {
            GetComponentInChildrenAttribute attr = attribute as GetComponentInChildrenAttribute;
            string name = attr.name;
            bool includeInactive = attr.includeInactive, nameNullOrEmpty = string.IsNullOrEmpty(name);

            if (Find(gameObject.transform, out Object target))
                property.objectReferenceValue = target;

            #region Local: Find(...)
            //=================================
            bool Find(Transform parent, out Object obj)
            {
                foreach (Transform child in parent)
                {
                    if (!includeInactive && !child.gameObject.activeSelf)
                        continue;

                    if (((nameNullOrEmpty || child.name == name) && (obj = child.GetComponent(type)) != null) || Find(child, out obj))
                        return true;
                }

                obj = null;
                return false;
            }
            #endregion
        }
    }
}
