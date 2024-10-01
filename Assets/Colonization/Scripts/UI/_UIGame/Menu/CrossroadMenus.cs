using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadMenus : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _canvasTransform;
        [Space]
        [SerializeField] private CrossroadMainMenu _mainMenu;
        [SerializeField] private CrossroadRoadsMenu _roadsMenu;

        private RectTransform _thisTransform;
        private Vector2 _localPoint;

        private void Start()
        {
            _thisTransform = GetComponent<RectTransform>();
            if (_camera == null)
                _camera = Camera.main;

            EventBus.Instance.EventCrossroadSelect += OnSelectCrossroad;

            _mainMenu.Initialize(_roadsMenu);
            _roadsMenu.Initialize(_mainMenu);

            #region Local: OnSelectCrossroad()
            //=================================
            void OnSelectCrossroad(Crossroad crossroad)
            {
                _roadsMenu.Close();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, _camera.WorldToScreenPoint(crossroad.Position), _camera, out _localPoint))
                    _thisTransform.anchoredPosition = _localPoint;

                _mainMenu.Open(crossroad);
            }
            #endregion
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_mainMenu == null)
                _mainMenu = GetComponentInChildren<CrossroadMainMenu>();
            if (_roadsMenu == null)
                _roadsMenu = GetComponentInChildren<CrossroadRoadsMenu>();
            if (_camera == null)
                _camera = Camera.main;
        }
#endif
    }
}
