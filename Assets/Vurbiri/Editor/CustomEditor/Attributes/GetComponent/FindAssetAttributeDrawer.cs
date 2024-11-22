//Assets\Vurbiri\Editor\CustomEditor\Attributes\GetComponent\FindAssetAttributeDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(FindAssetAttribute))]
    internal class FindAssetAttributeDrawer : AGetComponentAttributeDrawer
    {
        private const string PREFAB_TYPE = "t:Prefab";
        private static readonly string[] PATH = { "Assets" };

        protected override void SetProperty(SerializedProperty property, GameObject gameObject, System.Type type)
        {
            FindAssetAttribute attr = attribute as FindAssetAttribute;
            Object findObj = type.Is(typeof(MonoBehaviour)) ? FindPrefab(attr.fileName) : FindAsset(attr.fileName, type.Name);
            if (findObj != null)
                property.objectReferenceValue = findObj;

            #region Local: FindPrefab(), FindAsset(...)
            //=================================
            Object FindPrefab(string fileName)
            {
                string path, find = string.IsNullOrEmpty(fileName) ? PREFAB_TYPE : $"{fileName} {PREFAB_TYPE}";
                string[] guids = AssetDatabase.FindAssets(find, PATH);
                Object obj;

                foreach (var guid in guids)
                {
                    path = AssetDatabase.GUIDToAssetPath(guid);
                    obj = (AssetDatabase.LoadMainAssetAtPath(path) as GameObject).GetComponent(type);
                    if (obj != null)
                        return obj;
                }
                return null;
            }
            //=================================
            Object FindAsset(string fileName, string typeName)
            {
                string find = string.IsNullOrEmpty(fileName) ? $"t:{typeName}" : $"{fileName} t:{typeName}";
                string[] guids = AssetDatabase.FindAssets(find, PATH);
                Object obj;

                foreach (var guid in guids)
                {
                    obj = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid));
                    if (obj.GetType().Is(type))
                        return obj;
                }
                return null;
            }
            #endregion
        }
    }
}
