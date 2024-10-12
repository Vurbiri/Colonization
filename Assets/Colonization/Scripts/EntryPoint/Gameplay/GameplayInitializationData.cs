using UnityEngine;

namespace Vurbiri.Colonization
{
    public class GameplayInitializationData : MonoBehaviour
    {
        public Camera cameraMain;
        public IslandCreator islandCreator;
        public CameraController cameraController;
        public UI.ContextMenusWorld contextMenusWorld;
        [Space]
        public InputController.Settings inputControllerSettings;
        [Space]
        //test
        public bool isLoad;
        public Id<PlayerId> id;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (islandCreator == null)
                islandCreator = FindAnyObjectByType<IslandCreator>();

            if (cameraMain == null)
                cameraMain = FindAnyObjectByType<Camera>();

            if (cameraController == null)
                cameraController = FindAnyObjectByType<CameraController>();

            if (contextMenusWorld == null)
                contextMenusWorld = FindAnyObjectByType<UI.ContextMenusWorld>();
        }
#endif
    }
}
