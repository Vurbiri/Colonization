using UnityEditor;
using UnityEngine;
using Vurbiri;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class LandMeshSave
	{
        #region Consts
        private const string NAME = "Save Land Mesh", MENU = MENU_PATH + NAME;
        #endregion

        [MenuItem(MENU, false, 77)]
        private static void Command()
        {
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog(NAME, "The Mesh has not been created", "OK");
                return;
            }
            
            MeshFilter meshFilter = EUtility.FindObjectByName<MeshFilter>("Land");

            string path = UnityEditor.EditorUtility.SaveFilePanelInProject(NAME, "LandMesh_001", "mesh", "", "Assets/Colonization/Graphics/3D/");

            if (meshFilter == null | string.IsNullOrEmpty(path))
                Debug.LogWarning($"MeshFilter: {meshFilter}. Path: {path}");

            AssetDatabase.CreateAsset(meshFilter.sharedMesh, path);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog(NAME, $"Mesh {meshFilter.sharedMesh}\nPath: {path}", "OK");
        }

    }
}
