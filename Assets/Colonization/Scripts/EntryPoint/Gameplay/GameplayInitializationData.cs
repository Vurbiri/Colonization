using UnityEngine;

namespace Vurbiri.Colonization
{
    public class GameplayInitializationData : MonoBehaviourDisposable
    {
        public Camera cameraMain;
        public IslandCreator islandCreator;
        public CameraController cameraController;
        public UI.ContextMenusWorld contextMenusWorld;
        [Space]
        public InputController.Settings inputControllerSettings;
        [Space]
        public RoadsSetup road;
        [Space]
        public SurfacesScriptable surfaces;
        [Header("TEST")]
        public bool isLoad;
        public Id<PlayerId> id;


        #region Nested: Settings
        //***********************************
        [System.Serializable]
        public class RoadsSetup
        {
            public Roads prefab;
            public Transform container;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (cameraMain == null)
                cameraMain = FindAnyObjectByType<Camera>();

            if (islandCreator == null)
                islandCreator = FindAnyObjectByType<IslandCreator>();

            if (cameraController == null)
                cameraController = FindAnyObjectByType<CameraController>();

            if (contextMenusWorld == null)
                contextMenusWorld = FindAnyObjectByType<UI.ContextMenusWorld>();

            if (road.prefab == null)
                road.prefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();

            //if (surfaces == null)
            //    surfaces = VurbiriEditor.Utility.FindAnyScriptable<SurfacesScriptable>();
        }
#endif
    }
}
