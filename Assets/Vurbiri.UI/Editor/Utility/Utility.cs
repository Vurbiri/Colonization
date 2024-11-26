//Assets\Vurbiri.UI\Editor\Utility\Utility.cs
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VurbiriEditor
{
    public static class Utility
	{
        public static void CreateFromPrefab(string path, string name, GameObject parent) 
            => Place(GameObject.Instantiate(Resources.Load(path)) as GameObject, parent, name);

        public static GameObject CreateObject(string name, GameObject parent, params Type[] types)
        {
            GameObject newObject = ObjectFactory.CreateGameObject(name, types);
            Place(newObject, parent, name);

            return newObject;
        }

        private static void Place(GameObject gameObject, GameObject parent, string name)
        {
            gameObject.name = name;
            //gameObject.transform.parent = parent.transform;

            GameObjectUtility.SetParentAndAlign(gameObject, parent);

            // Find location
            //SceneView lastView = SceneView.lastActiveSceneView;
            //gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
