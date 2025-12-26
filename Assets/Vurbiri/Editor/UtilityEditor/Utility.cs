using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    public static class Utility
	{
        private static readonly FieldInfo s_fieldType = typeof(CustomPropertyDrawer).GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsUnityProperty(SerializedProperty property)
        {
            return property.propertyType != SerializedPropertyType.Generic;
        }

        public static bool IsCustomProperty(Type propertyType)
        {
            foreach (var type in TypeCache.GetTypesDerivedFrom<PropertyDrawer>())
                foreach (var attribute in type.GetCustomAttributes<CustomPropertyDrawer>(true))
                    if (propertyType.IsGeneric((Type)s_fieldType.GetValue(attribute)))
                        return true;
            return false;
        }

        public static void CreateObjectFromResources(string path, string name, GameObject parent) 
            => Place(GameObject.Instantiate(Resources.Load(path)) as GameObject, parent, name);

        public static void CreateObjectFromPrefab(MonoBehaviour prefab, string name, GameObject parent)
            => Place(GameObject.Instantiate(prefab).gameObject, parent, name);

        public static GameObject CreateObject(string name, GameObject parent, params Type[] types)
        {
            GameObject newObject = ObjectFactory.CreateGameObject(name, types);
            newObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            Place(newObject, parent, name);

            return newObject;
        }

        private static void Place(GameObject gameObject, GameObject parent, string name)
        {
            gameObject.name = name;

            GameObjectUtility.SetParentAndAlign(gameObject, parent);

            //SceneView lastView = SceneView.lastActiveSceneView;
            //gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
