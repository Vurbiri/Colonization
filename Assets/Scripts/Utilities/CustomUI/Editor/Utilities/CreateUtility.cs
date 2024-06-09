using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CreateUtility
{
    public static void Prefab(string path, string name, GameObject parent) => Place(PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject, parent, name);
    public static void FromPrefab(string path, string name, GameObject parent) => Place(GameObject.Instantiate(Resources.Load(path)) as GameObject, parent, name);

    public static GameObject Object(string name, GameObject parent, params Type[] types)
    {
        GameObject newObject = ObjectFactory.CreateGameObject(name, types);
        Place(newObject, parent, name);

        return newObject;
    }


    public static void Place(GameObject gameObject, GameObject parent, string name)
    {
        gameObject.name = name;
        //gameObject.transform.parent = parent.transform;

        GameObjectUtility.SetParentAndAlign(gameObject, parent);

        // Find location
        //SceneView lastView = SceneView.lastActiveSceneView;
        //gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

        // Make sure we place the object in the proper scene, with a relevant name
        StageUtility.PlaceGameObjectInCurrentStage(gameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

        // Record undo, and select
        Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
        Selection.activeGameObject = gameObject;

        // For prefabs, let's mark the scene as dirty for saving
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
